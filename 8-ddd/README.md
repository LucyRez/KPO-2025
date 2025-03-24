# Занятие 8. Domain-Driven Design (DDD)

## Цель занятия

- Понять основные принципы и концепции Domain-Driven Design
- Научиться применять DDD в практических проектах
- Проанализировать существующий проект с точки зрения DDD
- Научиться рефакторить код в соответствии с принципами DDD

## Теоретическая часть: Что такое Domain-Driven Design?

Domain-Driven Design (DDD) — это подход к разработке программного обеспечения, который фокусируется на моделировании бизнес-домена и создании программных абстракций, которые отражают бизнес-модель. DDD был предложен Эриком Эвансом в его книге "Domain-Driven Design: Tackling Complexity in the Heart of Software" (2003).

### Основные концепции DDD

#### 1. Единый язык (Ubiquitous Language)

Единый язык — это общий язык, используемый как разработчиками, так и экспертами в предметной области. Он помогает устранить недопонимание между техническими и бизнес-специалистами.

**Пример**: Вместо использования технических терминов вроде "Entity" или "Repository", мы используем термины из бизнес-домена, такие как "Покупатель", "Автомобиль", "Продажа".

#### 2. Ограниченный контекст (Bounded Context)

Ограниченный контекст — это граница, внутри которой определенная модель имеет конкретное значение. Разные контексты могут иметь разные модели для одних и тех же понятий.

**Пример**: Понятие "Клиент" в контексте продаж может отличаться от понятия "Клиент" в контексте бухгалтерии.

#### 3. Предметная область (Domain)

Предметная область — это сфера знаний, деятельности или интересов, к которой относится разрабатываемое приложение.

**Пример**: В нашем проекте предметная область — это продажа автомобилей.

#### 4. Модель предметной области (Domain Model)

Модель предметной области — это абстракция, которая описывает выбранные аспекты предметной области и может использоваться для решения проблем, связанных с этой предметной областью.

**Пример**: В нашем проекте модель включает в себя понятия "Покупатель", "Автомобиль", "Двигатель" и отношения между ними.

### Строительные блоки DDD

#### 1. Сущности (Entities)

Сущности — это объекты, которые имеют идентичность, не зависящую от их атрибутов. Две сущности могут быть равны, даже если их атрибуты различаются.

**Пример**: `Car` в нашем проекте является сущностью, так как имеет уникальный номер (идентификатор).

#### 2. Объекты-значения (Value Objects)

Объекты-значения — это объекты, которые не имеют идентичности и полностью определяются своими атрибутами.

**Пример**: В нашем проекте нет явных объектов-значений, но можно было бы ввести, например, `Address` для хранения адреса покупателя.

#### 3. Агрегаты (Aggregates)

Агрегат — это кластер объектов, которые рассматриваются как единое целое с точки зрения изменения данных. У каждого агрегата есть корень (Aggregate Root), через который осуществляется доступ к другим объектам агрегата.

**Пример**: В нашем проекте `Car` мог бы быть корнем агрегата, включающего в себя `Engine`.

#### 4. Репозитории (Repositories)

Репозиторий — это механизм для инкапсуляции хранения, поиска и извлечения объектов модели.

**Пример**: `CarService` и `CustomersStorage` в нашем проекте выполняют роль репозиториев.

#### 5. Сервисы предметной области (Domain Services)

Сервисы предметной области — это операции, которые не принадлежат ни к одной сущности или объекту-значению, но являются частью модели предметной области.

**Пример**: `HseCarService` в нашем проекте является сервисом предметной области, отвечающим за продажу автомобилей.

#### 6. Фабрики (Factories)

Фабрики — это объекты, отвечающие за создание сложных объектов или агрегатов.

**Пример**: `PedalCarFactory` и `HandCarFactory` в нашем проекте являются фабриками.

#### 7. События предметной области (Domain Events)

События предметной области — это объекты, которые фиксируют что-то, что произошло в предметной области.

**Пример**: В нашем проекте нет явных событий предметной области, но можно было бы ввести, например, `CarSoldEvent`.

## Анализ текущего проекта с точки зрения DDD

### Где мы соответствуем принципам DDD?

1. **Использование единого языка**: 
   - Наши классы и методы используют термины из предметной области: `Car`, `Customer`, `CarService`, `HseCarService`.
   - Комментарии к коду также используют термины из предметной области.

2. **Четкое разделение на модули**:
   - Проект разделен на модули по функциональности: `Cars`, `Customers`, `Sales`, `Reports`, `Accounting`.
   - Каждый модуль отвечает за свою часть функциональности.

3. **Использование фабрик**:
   - `PedalCarFactory` и `HandCarFactory` отвечают за создание автомобилей разных типов.

4. **Использование сервисов предметной области**:
   - `HseCarService` отвечает за бизнес-логику продажи автомобилей.

5. **Инкапсуляция**:
   - Сущности `Car` и `Customer` инкапсулируют свое состояние и поведение.
   - Некоторые свойства доступны только для чтения, что защищает инварианты.

### Где мы противоречим принципам DDD?

1. **Отсутствие явных агрегатов**:
   - В проекте нет явного определения агрегатов и их корней.
   - Отношения между сущностями не всегда четко определены.

2. **Смешение ответственностей**:
   - `CarService` выполняет функции как репозитория, так и сервиса предметной области.
   - `CustomersStorage` также смешивает ответственности.

3. **Отсутствие объектов-значений**:
   - В проекте не используются объекты-значения, хотя они могли бы упростить модель.

4. **Недостаточное использование инвариантов**:
   - Бизнес-правила не всегда явно выражены в коде.
   - Некоторые инварианты могут быть нарушены из-за отсутствия проверок.

5. **Отсутствие событий предметной области**:
   - В проекте не используются события предметной области, что затрудняет отслеживание изменений.

6. **Фасад как антипаттерн**:
   - `CarShop` является фасадом, который скрывает сложность системы, но также скрывает и модель предметной области.
   - Фасад может противоречить принципам DDD, если он скрывает важные бизнес-концепции.

## Как превратить наш проект в соответствующий DDD?

### 1. Определить ограниченные контексты

Разделить проект на несколько ограниченных контекстов:

- **Контекст управления автомобилями**: Все, что связано с автомобилями, их созданием и хранением.
- **Контекст управления покупателями**: Все, что связано с покупателями.
- **Контекст продаж**: Все, что связано с продажей автомобилей.
- **Контекст отчетности**: Все, что связано с созданием и экспортом отчетов.
- **Контекст планируемых операций**: Все, что связано с планированием операций.

### 2. Определить агрегаты и их корни

- **Агрегат автомобиля**: Корень - `Car`, включает в себя `Engine`.
- **Агрегат покупателя**: Корень - `Customer`.

### 3. Ввести объекты-значения

- **`EngineSpecification`**: Объект-значение для хранения характеристик двигателя.
- **`CustomerCapabilities`**: Объект-значение для хранения возможностей покупателя (сила ног, сила рук).

### 4. Разделить репозитории и сервисы

- **`CarRepository`**: Отвечает только за хранение и извлечение автомобилей.
- **`CustomerRepository`**: Отвечает только за хранение и извлечение покупателей.
- **`SalesService`**: Отвечает за бизнес-логику продажи автомобилей.

### 5. Ввести события предметной области

- **`CustomerAddedEvent`**: Событие, возникающее при добавлении покупателя.
- **`CarSoldEvent`**: Событие, возникающее при продаже автомобиля.

### 6. Улучшить инкапсуляцию и защиту инвариантов

- Добавить проверки в конструкторы и методы изменения состояния.
- Использовать приватные сеттеры и методы для изменения состояния.

### 7. Рефакторинг фасада

- Разделить `CarShop` на несколько сервисов приложения, каждый из которых отвечает за свою часть функциональности.

## Практический план рефакторинга

### Шаг 1: Введение объектов-значений
1. Создание объекта-значения `CustomerCapabilities`:
   ```csharp
   // Создаем новый файл CustomerCapabilities.cs в папке Customers
   namespace UniversalCarShop.Customers;

   public sealed record CustomerCapabilities
   {
       public int LegPower { get; }
       public int HandPower { get; }

       public CustomerCapabilities(int legPower, int handPower)
       {
           if (legPower < 0)
               throw new ArgumentException("Сила ног не может быть отрицательной", nameof(legPower));
           
           if (handPower < 0)
               throw new ArgumentException("Сила рук не может быть отрицательной", nameof(handPower));
           
           LegPower = legPower;
           HandPower = handPower;
       }
   }
   ```

2. Рефакторинг класса `Customer` для использования объекта-значения:
   ```csharp
   // Модифицируем класс Customer
   public sealed class Customer
   {
       public Customer(string name, CustomerCapabilities capabilities)
       {
           if (string.IsNullOrWhiteSpace(name))
               throw new ArgumentException("Имя не может быть пустым", nameof(name));
           
           Name = name;
           Capabilities = capabilities ?? throw new ArgumentNullException(nameof(capabilities));
       }
       
       public string Name { get; }
       public CustomerCapabilities Capabilities { get; }
       public Car? Car { get; private set; }

       // Метод для назначения автомобиля покупателю
       public void AssignCar(Car car)
       {
           Car = car;
       }

       // Переопределяем ToString
       public override string ToString()
       {
           var builder = new StringBuilder();
           builder.Append($"Имя: {Name}. Сила ног: {Capabilities.LegPower}. Сила рук: {Capabilities.HandPower}. ");
           if (Car is null)
           {
               builder.Append("Автомобиль: { Нет }");
           }
           else
           {
               builder.Append($"Автомобиль: {{ {Car} }}");
           }
           return builder.ToString();
       }
   }
   ```

#### Шаг 2: Создание объекта-значения для двигателя
1. Создание объекта-значения `EngineSpecification`:
   ```csharp
   // Создаем новый файл EngineSpecification.cs в папке Engines
   namespace UniversalCarShop.Engines;

   public sealed record EngineSpecification
   {
       public EngineSpecification(int requiredLegPower, int requiredHandPower, string type)
       {
           if (requiredLegPower < 0)
               throw new ArgumentException("Требуемая сила ног не может быть отрицательной", nameof(requiredLegPower));
           
           if (requiredHandPower < 0)
               throw new ArgumentException("Требуемая сила рук не может быть отрицательной", nameof(requiredHandPower));
           
           Type = type;
           RequiredLegPower = requiredLegPower;
           RequiredHandPower = requiredHandPower;
       }

       public int RequiredLegPower { get; }
       public int RequiredHandPower { get; }
       public string Type { get; }

       public bool IsCompatibleWith(CustomerCapabilities capabilities)
       {
           if (capabilities == null)
               throw new ArgumentNullException(nameof(capabilities));
           
           return capabilities.LegPower >= RequiredLegPower && 
                  capabilities.HandPower >= RequiredHandPower;
       }
   }
   ```

2. Рефакторинг интерфейса `IEngine` для использования объекта-значения:
   ```csharp
   // Модифицируем интерфейс IEngine
   public interface IEngine
   {
       EngineSpecification Specification { get; }
       bool IsCompatible(Customer customer);
   }
   ```

3. Рефакторинг реализаций интерфейса `IEngine`:
   ```csharp
   // Модифицируем класс PedalEngine
   public sealed class PedalEngine : IEngine
   {
       public PedalEngine(int pedalSize)
       {
           if (pedalSize <= 0)
               throw new ArgumentException("Размер педалей должен быть положительным числом", nameof(pedalSize));
           
           // Создаем спецификацию двигателя на основе размера педалей
           // Требуемая сила ног должна быть выше 5
           Specification = new EngineSpecification(
               requiredLegPower: 5, // Минимальная требуемая сила ног
               requiredHandPower: 0, // Для педального двигателя не требуется сила рук
               type: "Педальный"
           );
       }

       public EngineSpecification Specification { get; }

       public bool IsCompatible(CustomerCapabilities capabilities)
       {
           if (capabilities == null)
               throw new ArgumentNullException(nameof(capabilities));
           
           return Specification.IsCompatibleWith(capabilities);
       }

       public override string ToString() => $"Педальный двигатель (требуемая сила ног: {Specification.RequiredLegPower})";
   }

   // Модифицируем класс HandEngine
   public sealed class HandEngine : IEngine
   {
       public HandEngine()
       {
           // Требуемая сила рук должна быть выше 5
           Specification = new EngineSpecification(
               requiredLegPower: 0, // Для ручного двигателя не требуется сила ног
               requiredHandPower: 5, // Минимальная требуемая сила рук
               type: "С ручным приводом"
           );
       }

       public EngineSpecification Specification { get; }

       public bool IsCompatible(CustomerCapabilities capabilities)
       {
           if (capabilities == null)
               throw new ArgumentNullException(nameof(capabilities));
           
           return Specification.IsCompatibleWith(capabilities);
       }

       public override string ToString() => $"Ручной двигатель (требуемая сила рук: {Specification.RequiredHandPower})";
   }
   ```

### Шаг 3: Выделение агрегата `Car`
1. Рефакторинг класса `Car` для явного определения его как корня агрегата:
   ```csharp
   // Модифицируем класс Car
   public sealed class Car
   {
       private readonly IEngine _engine;

       public Car(IEngine engine, int number)
       {
           Number = number;
           _engine = engine;
       }

       public int Number { get; }
       public bool IsSold { get; private set; }
       public EngineSpecification EngineSpecification => _engine.Specification;

       public bool IsCompatible(CustomerCapabilities capabilities) => _engine.IsCompatible(capabilities);

       public void MarkAsSold() => IsSold = true;

       public override string ToString() => $"Номер: {Number}. Двигатель: {_engine}";
   }
   ```

### Шаг 4: Разделение репозиториев и сервисов
1. Создание интерфейса репозитория для автомобилей:
   ```csharp
   // Создаем новый файл ICarRepository.cs в папке Cars
   namespace UniversalCarShop.Cars;

   public interface ICarRepository
   {
       void Add(Car car);
       Car? FindCompatibleCar(CustomerCapabilities capabilities);
       IEnumerable<Car> GetAll();
   }
   ```

2. Рефакторинг `CarService` для реализации интерфейса репозитория:
   ```csharp
   // Модифицируем класс CarService
   public sealed class CarRepository : ICarRepository
   {
       private readonly List<Car> _cars = new();

       public IEnumerable<Car> GetAll() => _cars.AsReadOnly();

       public void Add(Car car) => _cars.Add(car);

       public Car? FindCompatibleCar(CustomerCapabilities capabilities) => _cars
           .FirstOrDefault(car => !car.IsSold && car.IsCompatible(capabilities));
   }
   ```

3. Также нам понадобится сервис для получения номеров, доступных для новых автомобилей.

    ```csharp
    public sealed class CarNumberService(ICarRepository carRepository)
    {
        public int GetNextNumber() => carRepository
            .GetAll()
            .Select(c => c.Number)
            .DefaultIfEmpty(0)
            .Max() + 1;
    }
    ```

3. Создаем новый интерфейс ICustomerRepository в папке Customers
   ```csharp
   public interface ICustomerRepository
   {
       IEnumerable<Customer> GetAll();
       Customer? GetByName(string name);
       void Add(Customer customer);
   }
   ```

4. Изменим класс `CustomersStorage` так, чтобы он реализовывал интерфейс `ICustomerRepository`, заодно переименовав его в `CustomerRepository`
   ```csharp
   public sealed class CustomerRepository : ICustomerRepository
   {
       private readonly List<Customer> _customers = new();

       public IEnumerable<Customer> GetAll() => _customers;

       public Customer? GetByName(string name) => _customers.FirstOrDefault(c => c.Name == name);

       public void Add(Customer customer) => _customers.Add(customer);
   }
   ```

### Шаг 5: Актуализация кода команд

1. Обновим класс `AddCustomerCommand` для использования нового репозитория и ValueObject:
    ```csharp
    public sealed class AddCustomerCommand(ICustomerRepository customerRepository, string name, int legPower, int handPower) : IAccountingSessionCommand
    {
        private readonly CustomerCapabilities _capabilities = new(legPower, handPower);

        public void Apply()
        {
            customerRepository.Add(new Customer(name, _capabilities));
        }

        public override string ToString() => $"Добавлен покупатель {name}. Сила ног: {legPower}. Сила рук: {handPower}.";
    }
    ```

2. Обновим класс `AddPedalCarCommand` для использования нового репозитория и ValueObject:

    ```csharp
    public sealed class AddPedalCarCommand(ICarRepository carRepository, CarNumberService carNumberService, PedalCarFactory pedalCarFactory, int pedalSize) : IAccountingSessionCommand
    {
        public void Apply()
        {
            var number = carNumberService.GetNextNumber();
            var carParams = new PedalEngineParams(pedalSize);
            var car = pedalCarFactory.CreateCar(carParams, number);
            carRepository.Add(car);
        }

        public override string ToString() => $"Добавлен педальный автомобиль. Размер педалей: {pedalSize}.";
    }
    ```

3. Обновим класс `AddHandCarCommand` для использования нового репозитория и ValueObject:

    ```csharp
    public sealed class AddHandCarCommand(ICarRepository carRepository, CarNumberService carNumberService, HandCarFactory handCarFactory) : IAccountingSessionCommand
    {
        public void Apply()
        {
            var number = carNumberService.GetNextNumber();
            var car = handCarFactory.CreateCar(EmptyEngineParams.DEFAULT, number);
            carRepository.Add(car);
        }

        public override string ToString() => $"Добавлен автомобиль с ручным приводом.";
    }
    ```

### Шаг 6: Внедрение событий предметной области
1. Создание базового интерфейса для событий предметной области. Для этого создадим новый интерфейс IDomainEvent в папке Domain:
   ```csharp
   public interface IDomainEvent
   {
       DateTime OccurredOn { get; }
   }
   ```
2. Создадим класс CarAddedEvent для события добавления автомобиля в папке Domain
   ```csharp
   public sealed record CarAddedEvent(Car Car, DateTime OccurredOn) : IDomainEvent;
   ```

3. Создадим класс CarSoldEvent для события продажи автомобиля в папке Domain
   ```csharp
   public sealed record CarSoldEvent(Car Car, Customer Customer, DateTime OccurredOn) : IDomainEvent;
   ```

4. Создадим класс CustomerAddedEvent для события добавления покупателя в папке Domain
   ```csharp
   public sealed record CustomerAddedEvent(Customer Customer, DateTime OccurredOn) : IDomainEvent;
   ```

5. Создадим интерфейс IDomainEventService в папке Domain
   ```csharp
   public interface IDomainEventService
   {
       void Raise(IDomainEvent domainEvent);
       event Action<IDomainEvent> OnDomainEvent;
   }
   ```

6. Создадим реализацию для IDomainEventService в папке Domain
   ```csharp
   public sealed class DomainEvents : IDomainEventService
   {
       public void Raise(IDomainEvent domainEvent)
       {
           OnDomainEvent?.Invoke(domainEvent);
       }

       public event Action<IDomainEvent>? OnDomainEvent;
   }
   ```

7. Модифицируем метод класс `CarRepository`, добавив зависимость от `IDomainEventService`
    ```csharp
    public sealed class CarRepository : ICarRepository
    {
        // ... существующий код

        private readonly IDomainEventService _domainEventService;

        public CarRepository(IDomainEventService domainEventService)
        {
            _domainEventService = domainEventService;
        }

        // ... существующий код
    }
    ```
8. После этого добавляем в метод `Add` в классе `CarRepository` вызов события `CarAddedEvent`
    ```csharp
    _domainEventService.Raise(new CarAddedEvent(car, DateTime.UtcNow));
    ```

9. Также необходимо добавить зависимость `IDomainEventService` в класс `SalesService`
   ```csharp
   public sealed class SalesService(ICarRepository carRepository, ICustomerRepository customerRepository, IDomainEventService domainEventService)
   {
       // ... существующий код
   }
   ```
10. После чего добавить в метод `SellCar` в классе `SalesService` вызов события `CarSoldEvent`
   ```csharp
   public bool SellCar(Customer customer, Car car)
   {
       // ... существующий код
       domainEventService.Raise(new CarSoldEvent(car, customer, DateTime.UtcNow));
       return true;
   }
   ```

11. После этого добавляем зависимость `IDomainEventService` в класс `CustomerRepository`
   ```csharp
   public sealed class CustomerRepository : ICustomerRepository
   {
       // ... существующий код
       private readonly IDomainEventService _domainEventService;

       public CustomerRepository(IDomainEventService domainEventService)
       {
           _domainEventService = domainEventService;
       }

       // ... существующий код
   }
   ```
12. И добавляем в метод `Add` в классе `CustomerRepository` вызов события `CustomerAddedEvent`
   ```csharp
   public void Add(Customer customer)
   {
       // ... существующий код
       _domainEventService.Raise(new CustomerAddedEvent(customer, DateTime.UtcNow));
   }
   ```

### Шаг 7: Рефакторинг системы учета

1. Переименуем класс `AccountingSession` в `PendingCommandService`

### Шаг 8: Рефакторинг фасада CarShop

1. Создаем новый класс CustomerService в папке Customers:
   ```csharp
   public sealed class CustomerService(PendingCommandService pendingCommandService, ICustomerRepository customerRepository)
   {
       public void AddCustomerPending(string name, int legPower, int handPower)
       {
           var command = new AddCustomerCommand(customerRepository, name, legPower, handPower);

           pendingCommandService.AddCommand(command);
       }
   }
   ```

2. Создаем новый класс CarInventoryService в папке Cars
    ```csharp
    public sealed class CarInventoryService(
        PendingCommandService pendingCommandService,
        ICarRepository carRepository,
        CarNumberService carNumberService,
        PedalCarFactory pedalCarFactory,
        HandCarFactory handCarFactory
    )
    {
        public void AddPedalCarPending(int pedalSize)
        {
            var command = new AddPedalCarCommand(carRepository, number, pedalSize);
            pendingCommandService.AddCommand(command);
        }

        public void AddHandCarPending()
        {
            var command = new AddHandCarCommand(carRepository, number);
            pendingCommandService.AddCommand(command);
        }
    }
    ```

3. Создаем новый класс SalesService в папке Sales
   ```csharp
    public sealed class SalesService(ICarRepository carRepository, ICustomerRepository customerRepository)
    {
        public void SellCars()
        {
            foreach (var customer in customerRepository.GetAll())
            {
                var car = carRepository.FindCompatibleCar(customer);

                if (car is not null)
                {
                    SellCar(customer, car);
                }
            }
        }

        public bool SellCar(Customer customer, Car car)
        {
            if (!car.IsCompatible(customer))
                return false;
            
            car.MarkAsSold();
            customer.AssignCar(car);
            
            DomainEvents.Raise(new CarSoldEvent(car, customer, DateTime.UtcNow));
            
            return true;
        }
    }
   ```

4. Создаем новый класс ReportingService в папке Reports
   ```csharp
    public sealed class ReportingService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICarRepository _carRepository;
        private readonly ReportBuilder _reportBuilder;
        private readonly ReportExporterFactory _reportExporterFactory;
        private readonly IDomainEventService _domainEventService;

        public ReportingService(
            ICustomerRepository customerRepository, 
            ICarRepository carRepository,
            ReportBuilder reportBuilder,
            ReportExporterFactory reportExporterFactory,
            IDomainEventService domainEventService)
        {
            _customerRepository = customerRepository;
            _carRepository = carRepository;
            _reportBuilder = reportBuilder;
            _reportExporterFactory = reportExporterFactory;
            _domainEventService = domainEventService;

            _domainEventService.OnDomainEvent += HandleDomainEvent;
        }

        public Report GetCurrentReport()
        {
            return _reportBuilder.Build();
        }

        public void ExportReport(ReportFormat format, TextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
                
            var report = GetCurrentReport();
            
            var exporter = _reportExporterFactory.Create(format);
            exporter.Export(report, writer);
        }

        private void HandleDomainEvent(IDomainEvent domainEvent)
        {
            if (domainEvent is CarSoldEvent carSoldEvent)
            {
                var car = carSoldEvent.Car;
                var customer = carSoldEvent.Customer;

                _reportBuilder.AddOperation(
                    $"Продажа: Автомобиль {car.Number} продан покупателю {customer.Name} " +
                    $"(сила ног: {customer.Capabilities.LegPower}, сила рук: {customer.Capabilities.HandPower}) " +
                    $"({carSoldEvent.OccurredOn})");
            }
            else if (domainEvent is CustomerAddedEvent customerAddedEvent)
            {
                var customer = customerAddedEvent.Customer;
                
                _reportBuilder.AddOperation(
                    $"Новый покупатель: {customer.Name} " +
                    $"(сила ног: {customer.Capabilities.LegPower}, сила рук: {customer.Capabilities.HandPower}) " +
                    $"({customerAddedEvent.OccurredOn})");
            }
            else if (domainEvent is CarAddedEvent carAddedEvent)
            {
                var car = carAddedEvent.Car;

                _reportBuilder.AddOperation($"Новый автомобиль {car.Number}. Тип двигателя: {car.EngineSpecification.Type} ({carAddedEvent.OccurredOn})");
            }
        }
    }
   ```

9. Так как логирование операций в отчет происходит в методе `HandleDomainEvent`, то удалим соответствующий код из `PendingCommandService`

### Шаг 9: Финальный этап

1. Обновление контейнера зависимостей

    ```csharp
    private static IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddSingleton<ICarRepository, CarRepository>(); // регистрируем репозиторий для автомобилей
        services.AddSingleton<ICustomerRepository, CustomerRepository>(); // регистрируем репозиторий для покупателей
        services.AddSingleton<PedalCarFactory>(); // регистрируем сервис для создания педальных автомобилей
        services.AddSingleton<HandCarFactory>(); // регистрируем сервис для создания автомобилей с ручным приводом
        services.AddSingleton<ReportExporterFactory>(); // регистрируем фабрику экспортеров
        services.AddSingleton<ReportBuilder>(); // регистрируем билдер отчетов
        services.AddSingleton<CarInventoryService>(); // регистрируем сервис для работы с автомобилями
        services.AddSingleton<CustomerService>(); // регистрируем сервис для работы с покупателями
        services.AddSingleton<SalesService>(); // регистрируем сервис для продажи автомобилей
        services.AddSingleton<ReportingService>(); // регистрируем сервис для отчетности
        services.AddSingleton<PendingCommandService>(); // регистрируем сервис для отложенных команд
        services.AddSingleton<CarNumberService>(); // регистрируем сервис для работы с номерами автомобилей
        services.AddSingleton<IDomainEventService, DomainEventService>(); // регистрируем сервис для работы с событиями предметной области

        services.AddSingleton(sp => new ReportServerClient("http://localhost:5000")); // регистрируем сервис для управления сервером отчетов

        return services.BuildServiceProvider();
    }
    ```

6. Также для удобства можно добавить в CompositionRoot свойства для доступа к нужной функциональности:

    ```csharp
    public static CustomerService CustomerService { get; } = Services.GetRequiredService<CustomerService>();
    public static CarInventoryService CarInventoryService { get; } = Services.GetRequiredService<CarInventoryService>();
    public static SalesService SalesService { get; } = Services.GetRequiredService<SalesService>();
    public static ReportingService ReportingService { get; } = Services.GetRequiredService<ReportingService>();
    public static PendingCommandService PendingCommandService { get; } = Services.GetRequiredService<PendingCommandService>();
    ```


7. Удаляем класс `CarShop`
8. Удаляем класс `HseCarService`

9. Обновляем `Program.cs`:

    ```csharp
    using UniversalCarShop;
    using UniversalCarShop.Reports;
    // Демонстрация работы с покупателями
    CompositionRoot.CustomerService.AddCustomer("Ваня", legPower: 6, handPower: 4);
    CompositionRoot.CustomerService.AddCustomer("Света", legPower: 4, handPower: 6);
    CompositionRoot.CustomerService.AddCustomer("Сергей", legPower: 6, handPower: 6);
    CompositionRoot.CustomerService.AddCustomer("Алексей", legPower: 4, handPower: 4);

    // Добавим лишнего покупателя и затем отменим действие
    CompositionRoot.CustomerService.AddCustomer("Маша", legPower: 4, handPower: 4);
    CompositionRoot.PendingCommandService.UndoLastCommand();

    // Демонстрация работы с автомобилями
    CompositionRoot.CarInventoryService.AddPedalCar(2);
    CompositionRoot.CarInventoryService.AddPedalCar(3);
    CompositionRoot.CarInventoryService.AddHandCar();
    CompositionRoot.CarInventoryService.AddHandCar();

    // Добавим лишний автомобиль и затем отменим действие
    CompositionRoot.CarInventoryService.AddPedalCar(4);
    CompositionRoot.PendingCommandService.UndoLastCommand();

    // Сохраняем внесенные изменения
    CompositionRoot.PendingCommandService.SaveChanges();

    // Демонстрация продажи автомобилей
    CompositionRoot.SalesService.SellCars();

    // Экспортируем отчет в формате Markdown
    CompositionRoot.ReportingService.ExportReport(ReportFormat.Markdown, Console.Out);

    // Экспортируем отчет в формате JSON
    using (var reportFile = new StreamWriter("report.json"))
    {
        CompositionRoot.ReportingService.ExportReport(ReportFormat.Json, reportFile);
    }

    // Экспортируем отчет в формате Server
    CompositionRoot.ReportingService.ExportReport(ReportFormat.Server, Console.Out);
    ```

## Проверка работы программы

Для проверки нам необходимо запустить сервер отчетов и затем запустить наше приложение.

Для запуска сервера отчетов запускаем проект `ReportServer` из каталога `misc/ReportServer` репозитория.
