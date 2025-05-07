# Занятие 10. Clean Architecture

## Теория

Clean Architecture - это принцип проектирования, который разделяет приложение в общем случае на 3 слоя:

- **Domain Layer** - слой сущностей и правил их взаимодействия.
- **Application Layer** - слой управления потоком данных.
- **Presentation & Infrastructure Layer** - слой, который отвечает за взаимодействие с пользователем и внешними системами.

Обычно в C# это выглядит следующим образом. Каждый слой имеет свой отдельный проект или проекты и предоставляет абстракции для других слоев.

## Практика

Для рассмотрения принципов Clean Architecture на практике мы превратим наше приложение по продаже автомобилей в веб-приложение.

Начнем с того, что определим наши слои:

- Доменный слой - содержит наши модели (автомобиль, покупатель, событие).
- Прикладной слой - содержит сервисы, реализующие бизнес-логику приложения.
- Слой представления - содержит контроллеры нашего веб-приложения.
- Инфраструктурный слой будет содержать реализации внешних сервисов, таких как база данных, внешние API и т.д.

### Доменный слой

Для определения слоя сущностей создадим проект `UniversalCarShop.Entities` и перенесем в него следующие типы:

- `Car` - модель автомобиля.
- `Customer` - модель покупателя.
- `IEngine` - интерфейс двигателя.
- `EngineSpecification` - спецификация двигателя.
- `CustomerCapabilities` - возможности покупателя.
- `PedalEngine` - модель педального двигателя.
- `HandEngine` - модель двигателя с ручным приводом.
- `IDomainEvent` - интерфейс события.
- `CarAddedEvent` - событие добавления автомобиля.
- `CarSoldEvent` - событие продажи автомобиля.
- `CustomerAddedEvent` - событие добавления покупателя.
- `Report` - модель отчета.

### Прикладной слой

Для определения слоя бизнес-правил создадим проект `UniversalCarShop.UseCases`, добавим в него ссылку на проект `UniversalCarShop.Entities` и перенесем в него типы, касающиеся различных аспектов нашего приложения.

#### Работа с автомобилями

- `ICarRepository` - интерфейс репозитория автомобилей.
- `ICarInventoryService` - интерфейс сервиса для работы с автомобилями.
- `CarInventoryService` - сервис для работы с автомобилями, реализующий `ICarInventoryService`, но при этом сменим уровень его видимости на `internal`.
- `ICarNumberService` - интерфейс сервиса для работы с номерами автомобилей.
- `CarNumberService` - сервис для работы с номерами автомобилей, реализующий `ICarNumberService`, но при этом сменим уровень его видимости на `internal`.
- `ICarFactory` - интерфейс фабрики автомобилей.
- `PedalCarFactory` - фабрика педальных автомобилей, реализующая `ICarFactory`, но при этом сменим уровень его видимости на `internal`.
- `HandCarFactory` - фабрика автомобилей с ручным приводом, реализующая `ICarFactory`, но при этом сменим уровень его видимости на `internal`.
- `EmptyEngineParams` - структура для случая, когда у двигателя нет каких-либо параметров. При этом переименуем ее в `HandEngineParams`.
- `PedalEngineParams` - параметры для создания педального двигателя.

#### Работа с покупателями

- `ICustomerRepository` - интерфейс репозитория покупателей.
- `ICustomerService` - интерфейс сервиса для работы с покупателями.
- `CustomerService` - сервис для работы с покупателями, реализующий `ICustomerService`, но при этом сменим уровень его видимости на `internal`.

#### Работа с событиями

- `IDomainEventService` - интерфейс сервиса для работы с событиями.
- `DomainEventService` - сервис для работы с событиями, реализующий `IDomainEventService`, но при этом сменим уровень его видимости на `internal`.

#### Работа с продажами

- `ISalesService` - интерфейс сервиса для работы с продажами.
- `SalesService` - сервис для работы с продажами, реализующий `ISalesService`, но при этом сменим уровень его видимости на `internal`.

#### Работа с отчетами

- `IReportingService` - интерфейс сервиса для работы с отчетами.
- `ReportingService` - сервис для работы с отчетами, реализующий `IReportingService`, но при этом сменим уровень его видимости на `internal`.
- `IReportExporterFactory` - интерфейс фабрики для экспорта отчетов.
- `ReportExporterFactory` - фабрика для экспорта отчетов, реализующая `IReportExporterFactory`, но при этом сменим уровень его видимости на `internal`.
- `ReportExporter` - экспортер отчетов.
- `ServerReportExporter` - экспортер отчетов в формате сервера, унаследованный от `ReportExporter`, но при этом сменим уровень его видимости на `internal`.
- `JsonReportExporter` - экспортер отчетов в формате JSON, унаследованный от `ReportExporter`, но при этом сменим уровень его видимости на `internal`.
- `MarkdownReportExporter` - экспортер отчетов в формате Markdown, унаследованный от `ReportExporter`, но при этом сменим уровень его видимости на `internal`.
- `ReportBuilder` - класс для построения отчета, но при этом сменим уровень его видимости на `internal`.
- `ReportFormat` - перечисление для форматов отчетов.

Здесь стоит отметить, что при переносе `ServerReportExporter` нам нужно избавиться от зависимости от `ReportServer.Client`. Для этого заведем интерфейс `IReportServerConnector`:

```csharp
public interface IReportServerConnector
{
    void StoreReport(Report report);
}
```

Этот интерфейс будем использовать вместо `ReportServerClient` в `ServerReportExporter`. Реализация этого интерфейса будет находиться в инфраструктурном слое.

Также в `ReportingService` добавим метод `Initialize`, который будет инициализировать сервис и подписываться на события `DomainEventService`.

```csharp
public void Initialize()
{
    _domainEventService.OnDomainEvent += HandleDomainEvent;
}
```

Для вызова данного метода добавим класс `ReportingServiceInitializer`, который будет реализовывать интерфейс `IHostedService` и вызывать метод `Initialize` при запуске приложения.

```csharp
internal sealed class ReportingServiceInitializer : IHostedService
{
    private readonly ReportingService _reportingService;

    public ReportingServiceInitializer(ReportingService reportingService)
    {
        _reportingService = reportingService;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _reportingService.Initialize();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
```

#### Работа с отложенными командами

- `IAccountingSessionCommand` - интерфейс отложенной команды.
- `IPendingCommandService` - интерфейс сервиса для работы с отложенными командами.
- `PendingCommandService` - сервис для работы с отложенными командами, реализующий `IPendingCommandService`, но при этом сменим уровень его видимости на `internal`.
- `AddCustomerCommand` - команда для добавления покупателя, реализующая `IAccountingSessionCommand`, но при этом сменим уровень его видимости на `internal`.
- `AddHandCarCommand` - команда для добавления автомобиля с ручным приводом, реализующая `IAccountingSessionCommand`, но при этом сменим уровень его видимости на `internal`.
- `AddPedalCarCommand` - команда для добавления автомобиля с педальным приводом, реализующая `IAccountingSessionCommand`, но при этом сменим уровень его видимости на `internal`.

#### Настройка DI-контейнера

Чтобы наши сервисы могли быть интегрированы с остальным приложением, нам необходимо зарегистрировать их в DI-контейнере. Для этого добавим в проект зависимости от следующих пакетов:
- `Microsoft.Extensions.DependencyInjection.Abstractions`,
- `Microsoft.Extensions.Hosting.Abstractions`.

После чего добавим статический класс `ServiceCollectionExtensions`, в котором определим метод `AddUseCases`, который будет регистрировать наши сервисы в DI-контейнере:

```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        // Сервисы для работы с автомобилями
        services.AddSingleton<ICarInventoryService, CarInventoryService>();
        services.AddSingleton<ICarNumberService, CarNumberService>();
        services.AddSingleton<ICarFactory<HandEngineParams>, HandCarFactory>();
        services.AddSingleton<ICarFactory<PedalEngineParams>, PedalCarFactory>();

        // Сервисы для работы с покупателями
        services.AddSingleton<ICustomerService, CustomerService>();

        // Сервисы для работы с событиями
        services.AddSingleton<IDomainEventService, DomainEventService>();

        // Сервисы для работы с продажами
        services.AddSingleton<ISalesService, SalesService>();

        // Сервисы для работы с отчетами
        services.AddSingleton<ReportingService>();
        services.AddSingleton<IReportingService>(sp => sp.GetRequiredService<ReportingService>());
        services.AddSingleton<IReportExporterFactory, ReportExporterFactory>();
        services.AddSingleton<ReportBuilder>();

        // Сервисы для работы с отложенными командами
        services.AddSingleton<IPendingCommandService, PendingCommandService>();

        // Инициализация сервисов
        services.AddHostedService<ReportingServiceInitializer>();
        
        return services;
    }
}
```

### Слой инфраструктуры

Для определения слоя инфраструктуры создадим проект `UniversalCarShop.Infrastructure` и добавим в него ссылку на проект `UniversalCarShop.UseCases`.

#### Работа с отчетами

Для реализации экспорта на сервер отчетов первым делом добавим реализацию интерфейса `IReportServerConnector`. Для этого добавим в проект ссылку на проект `ReportServer.Client`.

После этого добавим в проект `UniversalCarShop.Infrastructure` класс `ReportServerConnector`, реализующий интерфейс `IReportServerConnector`:

```csharp
using ExternalReport = ReportServer.Client.Report;
using Report = UniversalCarShop.Entities.Common.Report;

internal sealed class ReportServerConnector : IReportServerConnector
{
    private readonly ReportServerClient _reportServerClient;

    public ReportServerConnector(ReportServerClient reportServerClient)
    {
        _reportServerClient = reportServerClient;
    }

    public void StoreReport(Report report)
    {
        var externalReport = new ExternalReport
        {
            Title = report.Title,
            Contents = report.Content,
        };

        _reportServerClient.StoreReportAsync(externalReport).Wait();
    }
}
```

Регистрация данного сервиса в DI-контейнере будет рассмотрена позже.

#### Репозитории

Чтобы наше приложение могло хранить данные (пока что в памяти), перенесем в проект классы `CarRepository` и `CustomerRepository`, при этом сменив уровень видимости на `internal`.

#### Регистрация сервисов в DI-контейнере

Чтобы наши инфраструктурные сервисы могли быть интегрированы с остальным приложением, нам необходимо зарегистрировать их в DI-контейнере.

Для этого добавим ссылку на пакет `Microsoft.Extensions.DependencyInjection.Abstractions` и создадим класс `ServiceCollectionExtensions`, в котором добавим метод `AddInfrastructure`:

```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IReportServerConnector, ReportServerConnector>();
        services.AddSingleton<ICarRepository, CarRepository>();
        services.AddSingleton<ICustomerRepository, CustomerRepository>();
        
        return services;
    }
}
```

Внимательный читатель может заметить, что в методе мы зарегистрировли `ReportServerConnector`, но при этом не зарегистрировали `ReportServerClient`.

Дело в том, что для использования `ReportServerClient` нам требуется адрес сервера, который нам неоткуда взять в методе регистрации сервисов. Чтобы решать подобные проблемы, в ASP.NET Core существует механизм конфигурации приложения.

Чтобы им воспользоваться, добавим в проект ссылку на пакет `Microsoft.Extensions.Configuration`, после чего дополним нам метод `AddInfrastructure` следующим образом:

```csharp
public static class ServiceCollectionExtensions
{
    private const string ReportServerUrlPath = "ReportServer:Url";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // ... существующий код ...

        services.AddSingleton(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var reportServerUrl = configuration.GetSection(ReportServerUrlPath).Value
                ?? throw new InvalidOperationException($"Report server URL not found in configuration at path: {ReportServerUrlPath}");

            return new ReportServerClient(reportServerUrl);
        });

        return services;
    }
}
```

В данном фрагменте кода мы добавили в DI-контейнер сервис `ReportServerClient`, который будет проинициализирован с использованием адреса сервера,
указанного в поле `Url` в секции `ReportServer` конфигурации приложения.

### Слой представления

Для определения слоя представления создадим проект WEB API проект с контроллерами и назовем его `UniversalCarShop.Web`.

В него добавим ссылки на проекты `UniversalCarShop.UseCases` и `UniversalCarShop.Infrastructure`.

#### Контроллер для работы с автомобилями

Начнем с добавления контроллера для работы с автомобилями.

Создадим контроллер `CarsController` для работы с автомобилями.

```csharp
[Route("api/[controller]")]
[ApiController]
public class CarsController : ControllerBase
{
    private readonly ICarInventoryService _carInventoryService;

    public CarsController(ICarInventoryService carInventoryService)
    {
        _carInventoryService = carInventoryService;
    }
    
}
```

В данный класс добавим методы для добавления педального автомобиля и автомобиля с ручным приводом.

```csharp
[HttpPost("[action]")]
public IActionResult AddPedalCar([FromQuery] int pedalSize)
{
    _carInventoryService.AddPedalCarPending(pedalSize);
    return Ok();
}

[HttpPost("[action]")]
public IActionResult AddHandCar()
{
    _carInventoryService.AddHandCarPending();
    return Ok();
}
```

#### Контроллер для работы с покупателями

Создадим контроллер `CustomersController` для работы с покупателями.

```csharp
[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }
}
```

После чего добавим метод для добавления покупателя.

```csharp
[HttpPost]
public IActionResult AddCustomer(
    [FromQuery] string name,
    [FromQuery] int legPower,
    [FromQuery] int handPower
)
{
    _customerService.AddCustomerPending(name, legPower, handPower);
    return Ok();
}
```

#### Контроллер для работы с запланированными изменениями

Создадим контроллер `PendingChangesController` для работы с запланированными изменениями.

```csharp
[Route("api/[controller]")]
[ApiController]
public class PendingChangesController : ControllerBase
{
    private readonly IPendingCommandService _pendingCommandService;

    public PendingChangesController(IPendingCommandService pendingCommandService)
    {
        _pendingCommandService = pendingCommandService;
    }
}
```

После чего добавим методы для применения всех запланированных изменений и отмены последнего действия.

```csharp
[HttpPost("[action]")]
public IActionResult ApplyPendingChanges()
{
    _pendingCommandService.SaveChanges();
    return Ok();
}

[HttpPost("[action]")]
public IActionResult UndoLastCommand()
{
    _pendingCommandService.UndoLastCommand();
    return Ok();
}
```

#### Контроллер для работы с продажами

Создадим контроллер `SalesController` для работы с продажами.

```csharp
[Route("api/[controller]")]
[ApiController]
public class SalesController : ControllerBase
{
    private readonly ISalesService _salesService;

    public SalesController(ISalesService salesService)
    {
        _salesService = salesService;
    }
}
```

После чего добавим метод для продажи автомобилей.

```csharp
[HttpPost("[action]")]
public IActionResult SellCars()
{
    _salesService.SellCars();
    return Ok();
}
```
#### Контроллер для работы с отчетами

Создадим контроллер `ReportsController` для работы с отчетами.

```csharp
[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly IReportingService _reportingService;

    public ReportsController(IReportingService reportingService)
    {
        _reportingService = reportingService;
    }
}
```

После чего добавим методы для экспорта отчетов в различных форматах.

```csharp
[HttpGet("[action]")]
public IActionResult ExportJson()
{
    using var writer = new StringWriter();
    _reportingService.ExportReport(ReportFormat.Json, writer);
    var fileBytes = Encoding.UTF8.GetBytes(writer.ToString());
    return File(fileBytes, "application/json", "report.json");
}

[HttpGet("[action]")]
public IActionResult ExportMarkdown()
{
    using var writer = new StringWriter();
    _reportingService.ExportReport(ReportFormat.Markdown, writer);
    var fileBytes = Encoding.UTF8.GetBytes(writer.ToString());
    return File(fileBytes, "text/markdown", "report.md");
}

[HttpPost("[action]")]
public IActionResult ExportToServer()
{
    _reportingService.ExportReport(ReportFormat.Server, null!);
    return Ok();
}
```

#### Конфигурация приложения

по умолчанию в шаблоне ASP.NET Core уже настроен механизм конфигурации приложения. Поэтому чтобы добавить в настройки приложения Url сервера отчетов, нам необходимо добавить в файл `appsettings.json` следующие строки:

```json
{
    "ReportServer": {
        "Url": "http://localhost:5000"
    }
}
```

#### Конфигурация DI-контейнера

Для регистрации сервисов в DI-контейнере нам необходимо добавить в `Program.cs` после строчки `// Add services to the container.` следующие строки:

```csharp
// Add services to the container.

builder.Services.AddInfrastructure();
builder.Services.AddUseCases();
```

### Запуск приложения

Для запуска приложения нам необходимо запустить проект `UniversalCarShop.Web`.

```bash
dotnet run
```

В контроли будет написан адрес, по которому можно будет обращаться к приложению.

По умолчанию в приложениях ASP.NET Core настроен Swagger - поэтому можно добавить к данному адресу `/swagger` и получить доступ к удобному UI для тестирования нашего приложения.

При помощи добавленных нами методов проделаем те же манипуляции, что и в предыдущих примерах:

1. Добавим покупателя "Ваня" с силой ног 6 и силой рук 4.

2. Добавим покупателя "Света" с силой ног 4 и силой рук 6.

3. Добавим покупателя "Сергей" с силой ног 6 и силой рук 6.

4. Добавим покупателя "Алексей" с силой ног 4 и силой рук 4.

5. Добавим лишнего покупателя и затем отменим действие.

6. Добавим педальный автомобиль с размером педали 2.

7. Добавим педальный автомобиль с размером педали 3.

8. Добавим 2 автомобиля с ручным приводом.

9. Добавим лишний автомобиль и затем отменим действие.

10. Сохраняем внесенные изменения.

11. Продадим автомобили.

12. Экспортируем отчет в формате JSON.

13. Экспортируем отчет в формате Markdown.

14. Экспортируем отчет на сервер.

В отчет мы должны увидеть, примерно, следующее:

```
Операция: Новый покупатель: Ваня (сила ног: 6, сила рук: 4) (2025-03-15 12:00:00)
Операция: Новый покупатель: Света (сила ног: 4, сила рук: 6) (2025-03-15 12:00:00)
Операция: Новый покупатель: Сергей (сила ног: 6, сила рук: 6) (2025-03-15 12:00:00)
Операция: Новый покупатель: Алексей (сила ног: 4, сила рук: 4) (2025-03-15 12:00:00)
Операция: Новый автомобиль 1. Тип двигателя: Педальный (2025-03-15 12:00:00)
Операция: Новый автомобиль 2. Тип двигателя: Педальный (2025-03-15 12:00:00)
Операция: Новый автомобиль 3. Тип двигателя: С ручным приводом (2025-03-15 12:00:00)
Операция: Новый автомобиль 4. Тип двигателя: С ручным приводом (2025-03-15 12:00:00)
Операция: Продажа: Автомобиль 1 продан покупателю Ваня (сила ног: 6, сила рук: 4) (2025-03-15 12:00:00)
Операция: Продажа: Автомобиль 2 продан покупателю Света (сила ног: 4, сила рук: 6) (2025-03-15 12:00:00)
Операция: Продажа: Автомобиль 3 продан покупателю Сергей (сила ног: 6, сила рук: 6) (2025-03-15 12:00:00)
Операция: Продажа: Автомобиль 4 продан покупателю Алексей (сила ног: 4, сила рук: 4) (2025-03-15 12:00:00)
```



