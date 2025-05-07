# Занятие 11. Встраиваемые базы данных

## Теория

Встроенные базы данных - это базы данных, которые управляются самим приложением и не требуют отдельного сервера.

В C# наиболее популярной встроенной базой данных является SQLite.

SQLite - это встраиваемая SQL-база данных, написанная на языке C. Может быть использована практически в любом языке программирования, поддерживающем использование внешних библиотек.

Для работы с базами данных в C# существует множество различных решений, но наиболее популярным является использование ORM Entity Framework Core.

ORM - это специальное ПО, которое позволяет взаимодействовать с базой данных, используя объектно-ориентированный подход. Если говорить простым языком, то ORM позволяет преобразовывать объекты, хранимые в базе данных, в объекты в программе.

Фреймворк Entity Framework Core разрабатывается компанией Microsoft, является частью .NET и строится вокруг нескольких основных понятий:

- Контекст базы данных
- Модели
- Миграции

### Контекст базы данных

Контекст базы данных - это класс, который отвечает за взаимодействие с базой данных. Он содержит в себе настройки подключения к базе данных, а также методы для выполнения запросов к базе данных.

Контекст базы данных реализует паттерн Unit of Work. Данный паттерн позволяет нам сначала внести в памяти все необходимые изменения, а затем применить их к базе данных в рамках единой транзакции.

### Модели

Модели - это классы, которые описывают структуру таблиц в базе данных.

### Миграции

Миграции - это специальные классы, которые описывают изменения, вносимые в структуру базы данных.

## Практика

### Иммутабельность

Прежде чем приступить к работе с базой данных, необходимо подготовить наши классы, а именно:

- Подготовить их к сохранению в базу данных;
- Сделать возможность изменения состояния объектов только через репозитории.

#### Изменение класса Car

Начнем с того, что подготовим к сохранению наш класс Car. Для этого:

1. Сделаем Engine публичным свойством, так как без информации о двигателе мы не сможем ее сохранить:

```csharp
public class Car
{
    // ...
    public IEngine Engine { get; }
    // ...   
}
```

2. Добавим в конструктор Car параметр isSold, так как без этого невозможно будет полностью восстановить состояние автомобиля:

```csharp
public Car(IEngine engine, int number, bool isSold = false)
{
    // ...
    IsSold = isSold;
    // ...
}
```

3. Заменим метод MarkAsSold на метод Sell, который возвращает новый автомобиль с флагом `isSold = true`. Это позволит досигнуть иммутабельности автомобиля:

```csharp
public Car Sell() => new(Engine, Number, isSold: true);
```

#### Изменение класса Customer

Класс Car обновлен, теперь обновим класс Customer:

1. Добавим в конструктор Customer параметр Car, так как без этого невозможно будет полностью восстановить состояние покупателя:

```csharp
public Customer(string name, CustomerCapabilities capabilities, Car? car)
{
    // ...
    Car = car;
    // ...
}
```

2. Изменим метод AssignCar так, чтобы он возвращал нового покупателя, но с присовоенным ему автомобилем:

```csharp
public Customer AssignCar(Car car)
{
    return new Customer(Name, Capabilities, car);
}
```

#### Изменение репозитория покупателей

Так как теперь невозможно изменить состояние самого объекта, то для изменения состояния будут применяться репозитории. Добавим метод AssignCar в CustomerRepository, чтобы репозиторий сохранял внутри себя новую версию покупателя с назначенным автомобилем:

```csharp
public void AssignCar(Customer customer, Car car)
{
    _customers.Remove(customer);
    _customers.Add(customer.AssignCar(car));
}
```

#### Изменение сервиса продаж

Теперь изменим метод SellCar в SalesService так, чтобы он использовал новый метод AssignCar из репозитория:

```csharp
private bool SellCar(Customer customer, Car car)
{
    if (!car.IsCompatible(customer.Capabilities))
        return false;
    
    customerRepository.AssignCar(customer, car);
    domainEventService.Raise(new CarSoldEvent(car, customer, DateTime.UtcNow));
    return true;
}
```

Теперь и покупатели, и автомобили в нашей системе представлены в виде иммутабельных объектов, которые могут быть изменены только через репозитории.

### Работа с базой данных

Теперь рассмотрим, как добавить использование SQLite в наш проект и использовать в репозиториях данную СУБД.

В своем проекте мы будем использовать так называемый Code First подход, который предполагает написание кода моделей и контекста базы данных, а затем автоматическое создание базы данных с помощью Entity Framework Core.

1. Начнем с добавления зависимостей. Для этого переходим в каталог с инфраструктурным проектом и выполняем команды:

```bash
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0
```

Первый пакет нужен для использования SQLite в качестве СУБД, второй - для генерации миграций. Версия может варьироваться в зависимости от используемой версии .NET.

2. Далее добавим в наш инфраструктурный проект класс CustomerEntity, который будет представлять собой таблицу в базе данных для хранения покупателей:

```csharp
internal sealed record CustomerEntity(string Name, int LegPower, int HandPower, int? CarNumber)
{
    public int? CarNumber { get; set; } = CarNumber;
    public CarEntity? Car { get; set; }
}
```

3. Далее добавим в проект класс EngineEntityBase, который будет представлять собой базовый класс для хранения двигателей:

```csharp
internal abstract class EngineEntityBase
{
    public abstract IEngine DomainEngine { get; }
}
```

4. Далее добавим в проект класс PedalEngineEntity, который будет использоватья для представления педального двигателя в базе данных:

```csharp
internal sealed class PedalEngineEntity(int pedalSize) : EngineEntityBase
{
    public int PedalSize { get; set; } = pedalSize;
    [JsonIgnore]
    public override IEngine DomainEngine => new PedalEngine(PedalSize);
}
```

5. Далее добавим в проект класс HandEngineEntity, который будет использоватья для представления ручного двигателя в базе данных:

```csharp
internal sealed class HandEngineEntity : EngineEntityBase
{
    [JsonIgnore]
    public override IEngine DomainEngine => new HandEngine();
}
```

6. Двигатель будет храниться в формате JSON - поэтому настроим правильную сериализацию, добавив для класса EngineEntityBase JSON-атрибуты:

```csharp
[JsonDerivedType(typeof(PedalEngineEntity), nameof(PedalEngineEntity))]
[JsonDerivedType(typeof(HandEngineEntity), nameof(HandEngineEntity))]
internal abstract class EngineEntityBase
{
    // ...
    [JsonIgnore]
    public abstract IEngine DomainEngine { get; }
}
```

Атрибуты `JsonDerivedType` используются для указания того, какие классы являются производными для данного класса, чтобы сериализатор мог их правильно сериализовать и десериализовать. Атрибут `JsonIgnore` используется для указания того, что поле не должно быть сериализовано, так как свойство DomainEngine используется для правильного преобразования абстрактного двигателя из БД в конкретный доменный объект.

7. Далее добавим в проект класс CarEntity, который будет представлять собой таблицу в базе данных для хранения автомобилей:

```csharp
internal sealed record CarEntity(int Number, EngineEntityBase Engine);
```

8. Работа с БД в рамках EF Core происходит в контексте базы данных. Поэтому добавим в проект класс AppDbContext:

```csharp
internal sealed class AppDbContext : DbContext
{
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<CarEntity> Cars { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerEntity>(e =>
        {
            e.HasKey(c => c.Name);

            e.HasOne(c => c.Car)
                .WithOne()
                .HasForeignKey<CustomerEntity>(c => c.CarNumber);
        });

        modelBuilder.Entity<CarEntity>(e =>
        {
            e.HasKey(c => c.Number);

            e.Property(c => c.Engine)
                .HasConversion(
                    engine => JsonSerializer.Serialize<EngineEntityBase>(engine, _jsonSerializerOptions),
                    json => JsonSerializer.Deserialize<EngineEntityBase>(json, _jsonSerializerOptions)!
                );
        });
    }
}
```

Здесь много новых строук, на каждой из которых остановимся поподробнее.

С самого начала мы видим, что наш класс AppDbContext наследуется от класса DbContext, который является частью Entity Framework Core и обозначает, что наш класс является контекстом базы данных.

Далее мы видим, что в классе определены два свойства: Customers и Cars, которые имеют тип `DbSet<>`. `DbSet<>` - это класс, который представляет собой таблицу в базе данных в EF Core.

Далее мы видим конструктор класса, который принимает параметр `DbContextOptions<AppDbContext>`. Этот параметр используется для конфигурации подключения к базе данных.

Далее мы видим метод `OnModelCreating`, который используется для настройки модели базы данных.

В методе `OnModelCreating` сначала мы настраиваем таблицу Customers, указывая, что она имеет первичный ключ в виде свойства Name, а также имеет ссылку на таблицу Cars - таким образом устанавливается связь между покупателем и автомобилем на уровне базы данных.

Далее мы настраиваем таблицу Cars, указывая, что она имеет первичный ключ в виде свойства Number. Отношение с покупателями уже настроено ранее - поэтому второй раз не настраивается. Кроме ключа, здесь мы видим настройку сериализации свойства Engine - как мы ранее говорили, двигатель хранится в базе данных в формате JSON, поэтому нам нужно указать, как именно этот формат будет сериализоваться и десериализоваться.

9. Чтобы EF Core мог создать миграцию на основе нашего кода, необходимо добавить немного инфраструктрного кода, а именно фабрику контекста базы данных времени разработки:

```csharp
internal sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite("Data Source=:memory:");

        return new AppDbContext(optionsBuilder.Options);
    }
}
```

Здесь мы создаем контекст базы данных, который может быть использован EF Core при автоматической генерации миграции.

10. Теперь сгенерируем миграцию. Для этого сначала установим инструменты EF Core:

```bash
dotnet tool install --global dotnet-ef
```

Затем сгенерируем миграцию:

```bash
dotnet ef migrations add InitialMigration
```

Код миграции должен появиться в каталоге Migrations инфраструктурного проекта.

11. Миграции обычно применяют либо отдельно до развертывания приложения, либо автоматически при старте приложения. Мы будем использовать второй вариант. Для этого добавим в проект класс DatabaseMigrator, который будет применять миграции при старте приложения:

```csharp
public sealed class DatabaseMigrator : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public DatabaseMigrator(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
```

Класс DatabaseMigrator реализует интерфейс IHostedService, который позволяет нам запускать код при старте приложения.

В методе StartAsync мы создаем Scope для получения Scoped-сервисов, которым, как раз, является AppDbContext.

Далее мы применяем миграции к базе данных.

12. Теперь нам необходимо зарегистрировать все необходимые сервисы в DI-контейнере. Для этого добавим в проект класс ServiceCollectionExtensions:

```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // ...
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite("Data Source=UniversalCarShop.db");
        });

        services.AddHostedService<DatabaseMigrator>();
        
        // ...
    }
}
```

Вызов `AddDbContext<AppDbContext>` позволяет нам зарегистрировать контекст базы данных в DI-контейнере.

Вызов `AddHostedService<DatabaseMigrator>` позволяет нам зарегистрировать сервис DatabaseMigrator в DI-контейнере.

### Обновление репозиториев

Теперь перейдем к обновлению репозиториев.

1. Начнем с обновления регистрации некоторых сервисов в DI-контейнере. Так как контекст базы данных является Scoped-сервисов, то и все сервисы, которые его используют, должны быть зарегистрированы как Scoped. Как Scoped необходимо зарегистрировать следующие сервисы:

- ICustomerRepository
- ICarRepository
- ICarInventoryService
- ICarNumberService
- ICustomerService
- ISalesService

Для этого изменим вызов метода `AddSingletone` на вызов метода `AddScoped`:

```csharp
// ...
services.AddScoped<ICustomerRepository, CustomerRepository>();
services.AddScoped<ICarRepository, CarRepository>();
// ...
```

```csharp
// ...
services.AddScoped<ICarInventoryService, CarInventoryService>();
services.AddScoped<ICarNumberService, CarNumberService>();
services.AddScoped<ICustomerService, CustomerService>();
services.AddScoped<ISalesService, SalesService>();
// ...
```

2. Теперь добавим маппинги, чтобы более удобным образом конвертировать наши классы, используемые для работы с БД, в доменные объекты. Начнем с класса CustomerEntity и заведем в каталоге Mappings класс CustomerMappings:

```csharp
internal static class CustomerMappings
{
    public static Customer ToDomain(this CustomerEntity customerEntity) => new(
        name: customerEntity.Name,
        capabilities: new CustomerCapabilities(
            legPower: customerEntity.LegPower,
            handPower: customerEntity.HandPower
        ),
        car: customerEntity.Car?.ToDomain()
    );

    public static CustomerEntity ToEntity(this Customer customer) => new(
        Name: customer.Name,
        LegPower: customer.Capabilities.LegPower,
    );
}
```

Здесь мы использовали так называемые метода-расширения. Методы-расширения - это методы, которые позволяют добавлять новые методы в классы, не изменяя их исходный код. По сути, это синтаксический сахар, который позволяет использовать статические методы так, как будто они являются частью другого класса.

В методе `ToDomain` мы конвертируем объект `CustomerEntity` в объект `Customer`. В методе `ToEntity` мы конвертируем объект `Customer` в объект `CustomerEntity`.

3. Добавим аналогичный класс `CarMappings` для класса `CarEntity`:

```csharp
internal static class CarMappings
{
    public static Car ToDomain(this CarEntity carEntity) => new(
        number: carEntity.Number,
        engine: carEntity.Engine.DomainEngine
    );

    public static CarEntity ToEntity(this Car car) => new(
        Number: car.Number,
        Engine: car.Engine.ToEntity()
    );
}
```

4. И добавим класс `EngineMappings` для класса `EngineEntityBase`:

```csharp
internal static class EngineMappings
{
    public static EngineEntityBase ToEntity(this IEngine engine) => engine switch
    {
        PedalEngine pedalEngine => new PedalEngineEntity(pedalEngine.PedalSize),
        HandEngine handEngine => new HandEngineEntity(),
        _ => throw new ArgumentException("Unsupported engine type", nameof(engine))
    };
}
```

5. Теперь, когда маппинги настроены, обновим код репозиториев. Начнем с добавления зависимости от `AppDbContext` в конструктор `CustomerRepository`:

```csharp
internal sealed class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _dbContext;

    public CustomerRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // ...
}
```

6. Далее обновим методы получения данных:

```csharp
public IEnumerable<Customer> GetAll() => 
    _dbContext.Customers
        .Include(c => c.Car)
        .AsEnumerable()
        .Select(c => c.ToDomain());

public Customer? GetByName(string name) => _dbContext.Customers
    .Include(c => c.Car)
    .FirstOrDefault(c => c.Name == name)?
    .ToDomain();
```

Здесь мы видим использование объявленного нами ранее свойства `Customers` класса `AppDbContext` для доступа к таблице покупателей. Также при загрузке покупателей мы указываем при помощи метода `Include`, что нужно загрузить и автомобиль покупателя, если он есть. Также здесь мы видим применение метода маппинга, объявленного ранее.

7. Теперь обновим методы сохранения данных:

```csharp
public void Add(Customer customer)
{
    _dbContext.Customers.Add(customer.ToEntity());
    _dbContext.SaveChanges();

    _domainEventService.Raise(new CustomerAddedEvent(customer, DateTime.UtcNow));
}

public void AssignCar(Customer customer, Car car)
{
    _dbContext.Customers
        .Where(c => c.Name == customer.Name)
        .ExecuteUpdate(c => c.SetProperty(c => c.CarNumber, car.Number));
}
```

Здесь мы видим два подхода к записи информации в базу данных. Первый подход - это использование метода `Add` с последующим вызовом метода `SaveChanges`. Второй подход - это использование метода `ExecuteUpdate` для обновления существующей записи.

Первый подход удобен в случаях, когда нам нужно обновить или добавить несколько сущностей в одной транзакции. Второй подход удобен в случаях, когда нам нужно обновить или добавить только одну сущность, не загружая ее из базы данных.

8. Теперь обновим конструктор репозитория автомобилей:

```csharp
public CarRepository(IDomainEventService domainEventService, AppDbContext dbContext)
{
    // ...
    _dbContext = dbContext;
}
```

9. Далее обновим методы получения данных:

```csharp
public IEnumerable<Car> GetAll() => _dbContext.Cars.AsEnumerable().Select(c => c.ToDomain());

public Car? FindCompatibleCar(CustomerCapabilities capabilities)
{
    var query = (
        from car in _dbContext.Cars
        join customer in _dbContext.Customers on car.Number equals customer.CarNumber into customers
        from customer in customers.DefaultIfEmpty()
        where customer == null
        select car
    );

    return query
        .AsEnumerable()
        .Select(c => c.ToDomain())
        .FirstOrDefault(c => c.IsCompatible(capabilities));
}
```

Здесь в методе `FindCompatibleCar` мы используем LINQ для получения всех автомобилей, которые не имеют покупателя. Затем мы конвертируем результат в доменные объекты и возвращаем первый автомобиль, который совместим с возможностями покупателя.

10. Теперь обновим методы сохранения данных:

```csharp
public void Add(Car car)
{
    _dbContext.Cars.Add(car.ToEntity());
    _dbContext.SaveChanges();
    
    _domainEventService.Raise(new CarAddedEvent(car, DateTime.UtcNow));
}
```

### Получение покупателей

Для демонстрации функциональности сохранения данных добавим в контроллер CustomersController метод для получения всех покупателей.

1. Начнем с добавления DTO для автомобиля. Для этого в каталог DTOs добавим класс CarDto:

```csharp
public sealed record CarDto(
    int Number,
    string EngineType
);
```

2. Далее добавим DTO для покупателя в тот же каталог:

```csharp
public sealed record CustomerDto(
    string Name,
    int LegPower,
    int HandPower,
    CarDto? Car
);
```

3. Добавим новый метод в интерфейс ICustomerService:

```csharp
public interface ICustomerService
{
    // ...
    IEnumerable<CustomerDto> GetAllCustomers();
}
```

4. Теперь добавим реализацию метода GetAllCustomers в CustomerService:

```csharp
public IEnumerable<CustomerDto> GetAllCustomers() => customerRepository
    .GetAll()
    .Select(c => new CustomerDto(
        c.Name,
        c.Capabilities.LegPower,
        c.Capabilities.HandPower,
        c.Car is not null ? new CarDto(
            c.Car.Number,
            c.Car.Engine.Specification.Type) : null
    ));
```

5. И, напоследок, добавим метод GetAllCustomers в CustomersController:

```csharp
[HttpGet("[action]")]
public IActionResult GetAllCustomers()
{
    return Ok(_customerService.GetAllCustomers());
}
```

### Проверка работоспособности

1. Запустим приложение и проверим, что все работает корректно:

```bash
dotnet run
```

2. Попробуем добавить автомобиль, после чего сохранить изменения

3. Если все сделано верно, то мы получим ошибку, потому что наши команды ссылаются на репозитории, которые имеют Scoped время жизни, то есть уничтожаются сразу после окончания запроса, в котором были созданы - для решения этой проблемы мы можем использовать IServiceScopeFactory, который позволяет использовать Scoped-сервисы вне запроса.

### Исправление команд

1. Начнем в изменения в классе AddCustomerCommand:

```csharp
internal sealed class AddCustomerCommand(
    IServiceScopeFactory serviceScopeFactory,
    string name,
    int legPower,
    int handPower
) : IAccountingSessionCommand
{
    // ...

    public void Apply()
    {
        using var scope = serviceScopeFactory.CreateScope();
        var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
        customerRepository.Add(new Customer(name, _capabilities));
    }

    // ...
}
```

2. Далее обновим AddHandCarCommand:

```csharp
internal sealed class AddHandCarCommand(
    IServiceScopeFactory serviceScopeFactory,
    ICarFactory<HandEngineParams> handCarFactory
) : IAccountingSessionCommand
{
    // ...

    public void Apply()
    {
        using var scope = serviceScopeFactory.CreateScope();
        var carNumberService = scope.ServiceProvider.GetRequiredService<ICarNumberService>();
        var carRepository = scope.ServiceProvider.GetRequiredService<ICarRepository>();
        
        var number = carNumberService.GetNextNumber();
        var car = handCarFactory.CreateCar(HandEngineParams.DEFAULT, number);
        carRepository.Add(car);
    }

    // ...
}
```

3. Далее обновим AddPedalCarCommand:

```csharp
internal sealed class AddPedalCarCommand(
    IServiceScopeFactory serviceScopeFactory,
    ICarFactory<PedalEngineParams> pedalCarFactory,
    int pedalSize
) : IAccountingSessionCommand
{
    // ...

    public void Apply()
    {
        using var scope = serviceScopeFactory.CreateScope();
        var carNumberService = scope.ServiceProvider.GetRequiredService<ICarNumberService>();
        var carRepository = scope.ServiceProvider.GetRequiredService<ICarRepository>();
        
        var number = carNumberService.GetNextNumber();
        var carParams = new PedalEngineParams(pedalSize);
        var car = pedalCarFactory.CreateCar(carParams, number);
        carRepository.Add(car);
    }

    // ...
}
```

4. Далее обновим CarInventoryService:

```csharp
internal sealed class CarInventoryService(
    IServiceScopeFactory serviceScopeFactory,
    IPendingCommandService pendingCommandService,
    ICarFactory<PedalEngineParams> pedalCarFactory,
    ICarFactory<HandEngineParams> handCarFactory
) : ICarInventoryService
{
    public void AddPedalCarPending(int pedalSize)
    {
        var command = new AddPedalCarCommand(serviceScopeFactory, pedalCarFactory, pedalSize);
        pendingCommandService.AddCommand(command);
    }

    public void AddHandCarPending()
    {
        var command = new AddHandCarCommand(serviceScopeFactory, handCarFactory);
        pendingCommandService.AddCommand(command);
    }
}
```

5. Далее обновим CustomerService:

```csharp
internal sealed class CustomerService(
    IServiceScopeFactory serviceScopeFactory,
    IPendingCommandService pendingCommandService,
    ICustomerRepository customerRepository
) : ICustomerService
{
    public void AddCustomerPending(string name, int legPower, int handPower)
    {
        var command = new AddCustomerCommand(serviceScopeFactory, name, legPower, handPower);
        pendingCommandService.AddCommand(command);
    }

    // ...
}
```

6. Теперь при запуске приложения и проверке работоспособности мы больше не должны получать ошибку.
