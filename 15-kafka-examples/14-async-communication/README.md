# Занятие 14. Асинхронное межсервисное взаимодействие

## Теория

В общем случае азимодействие между сервисами делят на два типа:

- Синхронное
- Асинхронное

Синхронное взаимодействие подразумевает отправку запроса и ожидание подтверждения его обработки.

Асинхронное взаимодействие подразумевает отправку запроса и продолжение работы без ожидания подтверждения.

Обычно синхронное взаимодействие реализуется через HTTP-запросы, а асинхронное - через очереди сообщений, но на практике всё зависит от конкретной задачи и имеющихся вводных.

Очереди сообщений, как и следует из их названия, позволяют отправлять и получать сообщения между сервисами. Причем, так как это очереди, то это значит, что при отправке сообщения встают в очередь и ждут, пока какой-то другой сервис не заберет их из очереди. За счет этого и организуется асинхронное взаимодействие между сервисами.

Наиболее распространенными очередями сообщений являются Apache Kafka и RabbitMQ.

В данном занятии мы рассмотрим пример асинхронного взаимодействия между нашим основным сервисом и сервисом отчетов - организуем передачу доменных событий через очередь сообщений на основе Kafka.

Kafka - это распределенная платформа для потоковой передачи данных. Она позволяет отправлять и получать сообщения между сервисами в реальном времени.

Все сообщения в Kafka хранятся в топиках (topics). Узлы, которые записывают сообщения в топик, называются продюсерами (producers), а узлы, которые читают сообщения из топика, называются консьюмерами (consumers).

Консьюмеры могут подписываться на один или несколько топиков и получать все сообщения из них.

## Практика

### Шаг 1. Добавляем сервис для очереди сообщений

Начнем с того, что развернем Kafka. Для этого добавим соответствующий сервис в наш docker-compose.yml:

```yaml
services:
  # ... остальные сервисы ...
  kafka:
    image: confluentinc/cp-kafka:latest
    ports:
      - "9092:9092"
    expose:
      - "29092"
      - "9093"
    environment:
      ALLOW_PLAINTEXT_LISTENER: yes
      KAFKA_LISTENER_SECURITY_PROTOCOL_MAP: CLIENT:PLAINTEXT, LOCALHOST:PLAINTEXT, SERVICE:PLAINTEXT
      KAFKA_INTER_BROKER_LISTENER_NAME: CLIENT
      KAFKA_ADVERTISED_LISTENERS: CLIENT://kafka:29092, LOCALHOST://localhost:9092, SERVICE://kafka:9093
      KAFKA_LISTENERS: CLIENT://kafka:29092,LOCALHOST://0.0.0.0:9092,SERVICE://0.0.0.0:9093
      KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR: 1
      KAFKA_ZOOKEEPER_CONNECT: zookeeper:2181
  zookeeper:
    image: confluentinc/cp-zookeeper:latest
    ports:
      - "2181:2181"
    environment:
      ZOOKEEPER_CLIENT_PORT: 2181
  kafka-init:
    image: confluentinc/cp-kafka:latest
    depends_on:
      - kafka
    entrypoint: ["sh", "-c"]
    command: |
      "
      kafka-topics --bootstrap-server kafka:29092 --create --if-not-exists --topic car-shop-reported-events --partitions 1 --replication-factor 1
      "
```

Как мы видим, мы добавили не только сервис Kafka, но и Zookeeper - он используется Kafka для хранения метаданных и синхронизации их между различными экземплярами Kafka.

Кроме Kafka и Zookeeper, мы добавили сервис `kafka-init`, который создает топик `car-shop-reported-events` при запуске.

Теперь запустим наши сервисы с помощью docker-compose:

```bash
docker-compose up -d kafka kafka-init zookeeper
```

### Шаг 2. Добавим зависимости для работы с Kafka в наши инфраструктурные проекты

Для этого перейдем в каталог с решением и выполним команды:

```bash
dotnet add ./MainApp/UniversalCarShop.Infrastructure package Confluent.Kafka
dotnet add ./ReportService/ReportService.Infrastructure package Confluent.Kafka
```

### Шаг 3. Реализуем запись доменных событий в Kafka

Теперь, когда у нас есть работающий экземпляр Kafka, мы можем реализовать запись доменных событий в него.

Для этого в основном приложении найдем класс `ReportServerConnector` и реализуем в нем запись доменных событий в Kafka:

```csharp
internal sealed class ReportServerConnector : IReportServerConnector
{
    private readonly IProducer<string, string> _producer;
    private readonly string _topic;
    
    public ReportServerConnector(
        IProducer<string, string> producer,
        string topic)
    {
        _producer = producer;
        _topic = topic;
    }

    public void SendEvent(ReportedEventDto reportedEventDto)
    {
        var message = new Message<string, string> { Value = JsonSerializer.Serialize(reportedEventDto) };

        _producer.Produce(_topic, message);
    }
}
```

Как мы видим, вместо отправки запроса через HTTP-клиент, мы, используя интерфейс `IProducer` из библиотеки `Confluent.Kafka`, отправляем сообщение в Kafka. При этом название топика передается в конструктор класса для большей гибкости.

Теперь обновим код регистрации нашего коннектора в классе `ServiceCollectionExtensions`. При его регистрации посмотрим на примере того, как можно использовать обычные классы для конфигурации сервисов.

Для этого добавим в наш инструктурный проект зависимость `Microsoft.Extensions.Configuration.Binder`:

```bash
dotnet add ./MainApp/UniversalCarShop.Infrastructure package Microsoft.Extensions.Configuration.Binder --version 8
```

Теперь добавим новый класс для конфигурации нашего коннектора:

```csharp
internal sealed record ReportServerConnectorOptions(
    string Topic,
    string BootstrapServers
);
```

Теперь перейдем к коду регистрации нашего коннектора в классе `ServiceCollectionExtensions`. Начнем с добавления отдельного метода `AddReportServerConnector`:

```csharp
private static IServiceCollection AddReportServerConnector(this IServiceCollection services)
{
    services.AddSingleton<IReportServerConnector>(sp =>
    {
        var options = sp.GetRequiredService<IConfiguration>()
            .GetSection(ReportServerSectionPath)
            .Get<ReportServerConnectorOptions>() ?? throw new InvalidOperationException("Report server options not found");

        var producer = new ProducerBuilder<string, string>(new ProducerConfig { BootstrapServers = options.BootstrapServers }).Build();

        return new ReportServerConnector(producer, options.Topic);
    });

    return services;
}
```

Как мы видим, в данном методе мы извлекаем из конфигурации опции для нашего коннектора, после чего строим на основе них продьюсер и на основе продьюсера создаем экземпляр нашего коннектора.

Теперь добавим вызов этого метода в метод `AddInfrastructure` вместо текущего кода регистрации коннектора:

```csharp
services.AddReportServerConnector();
```

Теперь наш коннектор будет регистрироваться вместе с другими сервисами.

Чтобы мы могли запустить наше основное приложение, нам нужно также обновить его конфигурацию. Для этого добавим в файл `appsettings.json` соответствующие параметры:

```json
"ReportServer": {
  "Topic": "car-shop-reported-events",
  "BootstrapServers": "localhost:9092"
}
```

После этого также обновим конфигурацию основного приложения в `docker-compose.yml`. Для этого в секции `environment` сервиса `universal-car-shop` заменим параметр `REPORTSERVER__URL` на следующие параметры:

```yaml
REPORTSERVER__TOPIC: car-shop-reported-events
REPORTSERVER__BOOTSTRAPSERVERS: kafka:9093
```

Также в секции `depends_on` сервиса `universal-car-shop` заменим элемент `report-service` на `kafka`, так как мы больше не зависим от сервиса отчетов.

```yaml
depends_on:
  - postgres
  - kafka
```

### Шаг 4. Реализуем чтение доменных событий из Kafka

Теперь, когда мы можем записывать доменные события в Kafka, нам нужно научиться их читать. Так как Kafka является частью внешней инфраструктуры, то и работать с ней мы будем в инфраструкторном проекте сервиса отчетов.

Обычно для этого используется фоновый сервис, который читает сообщения из Kafka и обрабатывает их. Чтобы мы могли использовать фоновые сервисы в нашем проекте, нам нужно добавить в него библиотеку `Microsoft.Extensions.Hosting.Abstractions`:

```bash
dotnet add ./ReportService/ReportService.Infrastructure package Microsoft.Extensions.Hosting.Abstractions --version 8
```

После этого добавим каталог `ReportedEvents` и в нем класс `ReportedEventConsumer`:

```csharp
internal sealed class ReportedEventConsumer : BackgroundService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly string _topic;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ReportedEventConsumer(
        IConsumer<string, string> consumer,
        string topic,
        IServiceScopeFactory serviceScopeFactory)
    {
        _consumer = consumer;
        _topic = topic;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        _consumer.Subscribe(_topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            var result = _consumer.Consume(stoppingToken);

            if (result.IsPartitionEOF)
            {
                continue;
            }

            var reportedEvent = JsonSerializer.Deserialize<ReportedEventInDto>(result.Message.Value);
            
            if (reportedEvent is null)
            {
                continue;
            }

            await HandleReportedEventAsync(reportedEvent);
        }
    }

    private async Task HandleReportedEventAsync(ReportedEventInDto reportedEvent)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var reportedEventService = scope.ServiceProvider.GetRequiredService<IReportedEventService>();
        await reportedEventService.AddReportedEventAsync(reportedEvent);
    }
}
```

Как мы видим, в данном классе мы используем интерфейс `IConsumer` из библиотеки `Confluent.Kafka` для чтения сообщений из Kafka.

При старте сервиса мы подписываемся на переданный в конструктор топик и начинаем читать сообщения из него. Если достигаем конца очереди, то пропускаем сообщение.

Если сообщение корректно десериализуется, то обрабатываем его в методе `AddReportedEventAsync` сервиса `IReportedEventService` из прикладного слоя.

Теперь полученный консьюмер нужно зарегистрировать в нашем DI-контейнере. Для конфигурирования будем использовать тот же подход, что и для основного приложения. Начнем с добавления необходимой зависимости:

```bash
dotnet add ./ReportService/ReportService.Infrastructure package Microsoft.Extensions.Configuration.Binder --version 8
```

После этого добавляем класс для конфигурации:

```csharp
internal sealed record ReportedEventConsumerOptions(
    string Topic,
    string BootstrapServers
);

Далее в классе `ServiceCollectionExtensions` инфраструктурного проекта добавим метод `AddReportedEventConsumer`:

```csharp
private static IServiceCollection AddReportedEventConsumer(this IServiceCollection services)
{
    services.AddHostedService(sp =>
    {
        var options = sp.GetRequiredService<IConfiguration>()
            .GetSection(ReportedEventsConsumerSectionPath)
            .Get<ReportedEventConsumerOptions>() ?? throw new InvalidOperationException("Reported events consumer options not found");

        var consumer = new ConsumerBuilder<string, string>(new ConsumerConfig {
            BootstrapServers = options.BootstrapServers,
            GroupId = "report-service-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        }).Build();
        
        var serviceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();

        return new ReportedEventConsumer(consumer, options.Topic, serviceScopeFactory);
    });

    return services;
}
```

Как видим, как и в случае с основным приложением, мы извлекли опции для нашего консьюмера из конфигурации и на основе них создали консьюмер. После этого уже на основе консьюмера из `Confluent.Kafka` создаем фоновый сервис.

Теперь добавим вызов данного метода в метод `AddInfrastructure` вместо текущего кода регистрации консьюмера:

```csharp
services.AddReportedEventConsumer();
```

Чтобы всё заработало, нам нужно добавить соответствующие параметры в конфигурацию нашего сервиса отчетов в `appsettings.json`:

```json
"ReportedEventsConsumer": {
  "Topic": "car-shop-reported-events",
  "BootstrapServers": "localhost:9092"
}
```

Также нам нужно обновить конфигурацию нашего сервиса отчетов в `docker-compose.yml`. Для этого в секцию `environment` сервиса `report-service` добавим следующие параметры:

```yaml
REPORTSERVER__TOPIC: car-shop-reported-events
REPORTSERVER__BOOTSTRAPSERVERS: kafka:9093
```

Здесь важно обратить внимание, чтобы топик и адрес сервера Kafka совпадали с аналогичными параметрами в конфигурации основного приложения.

Также в секцию `depends_on` сервиса `report-service` добавим сервис `kafka`:

```yaml
depends_on:
- kafka
```

Теперь запустим наше приложение и проверим, что события из основного приложения успешно записываются в Kafka и обрабатываются сервисом отчетов. Для этого выполняем команду:

```bash
docker compose up
```

Когда всё завпустится, переходим по адресу `http://localhost:8080/swagger/index.html` и выполняем какие-либо действия, после чего переходим по адресу `http://localhost:8081/swagger/index.html` и проверяем, что события из основного приложения успешно записываются в Kafka и обрабатываются сервисом отчетов.
