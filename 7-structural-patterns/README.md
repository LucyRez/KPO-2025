# Занятие 7. Структурные паттерны

## Цель занятия

- Научиться использовать структурные паттерны

## Описание

Структурные паттерны предназначены для улучшения взаимодействия между классами.

В данном занятии мы рассмотрим следующие паттерны:

- Фасад
- Адаптер

## Фасад

Фасад - это структурный паттерн проектирования, который предоставляет простой интерфейс для взаимодействия с сложной системой.

Если взглянуть на наш файл `Program.cs`, то можно заметить, что он содержит в себе достаточно много кода, который периодически дублирует сам себя и имеет в себе довольно много различных элементов. Фасад позволит нам упростить данный код.

Заведем класс `CarShop` и переместим в него весь код, который отвечает за взимодействие с различными компонентами нашего приложения.

Начнем с заведения самого класса:

```csharp
public sealed class CarShop
{
    
}
```

Теперь добавим ссылки на все необходимые нам компоненты:

```csharp
private readonly CustomersStorage _customersStorage;
private readonly CarService _carService;
private readonly PedalCarFactory _pedalCarFactory;
private readonly HandCarFactory _handCarFactory;
private readonly HseCarService _hseCarService;
private readonly ReportBuilder _reportBuilder;
private readonly AccountingSession _accountingSession;
```

Как видим, у нас, действительно, много различных компонентов, про которые необходимо помнить.

Теперь добавим в класс `CarShop` методы для работы с системой учета:

```csharp
public void AddCustomer(string name, int legPower, int handPower)
{
    _accountingSession.AddCommand(new AddCustomerCommand(
        _customersStorage,
        name,
        legPower,
        handPower
    ));
}

public void AddPedalCar(int pedalSize)
{
    _accountingSession.AddCommand(new AddPedalCarCommand(
        _pedalCarFactory,
        _carService,
        pedalSize
    ));
}

public void AddHandCar()
{
    _accountingSession.AddCommand(new AddHandCarCommand(
        _handCarFactory,
        _carService
    ));
}

public void SaveCarsAndCustomers()
{
    _accountingSession.SaveChanges();
}

public void UndoLastAccountingAction()
{
    _accountingSession.UndoLastCommand();
}
```

Далее добавим методы для продажи автомобилей и вывода списка покупателей:

```csharp
public void SellCars()
{
    _hseCarService.SellCars();

    _reportBuilder
        .AddOperation("Продажа автомобилей")
        .AddCustomers(_customersStorage.GetCustomers());
}

public void ShowCustomers()
{
    _reportBuilder
        .AddOperation("Вывод списка покупателей")
        .AddCustomers(_customersStorage.GetCustomers());
}
```

Теперь добавим метод для экспорта отчета:

```csharp
public void ExportReport(ReportFormat format, TextWriter writer)
{
    var exporter = _reportExporterFactory.Create(format);

    exporter.Export(_reportBuilder.Build(), writer);
}
```

Зарегистрируем наш фасад в DI-контейнере. Для этого в методе `CreateServiceProvider` класса `CompositionRoot` добавим следующий код:

```csharp
services.AddSingleton<CarShop>();
```

Теперь мы можем использовать наш фасад в любом месте нашего приложения, а именно в `Program.cs`.

Заменим получение всех зависимостей из DI-контейнера на получение фасада:

```csharp
var carShop = services.GetRequiredService<CarShop>();
```

Теперь мы можем использовать фасад для взаимодействия с различными компонентами нашего приложения.

```csharp
// добавляем покупателей
carShop.AddCustomer("Ваня", legPower: 6, handPower: 4);
carShop.AddCustomer("Света", legPower: 4, handPower: 6);
carShop.AddCustomer("Сергей", legPower: 6, handPower: 6);
carShop.AddCustomer("Алексей", legPower: 4, handPower: 4);

// Добавим лишнего покупателя
carShop.AddCustomer("Маша", legPower: 4, handPower: 4);

// отменяем последнюю команду
carShop.UndoLastAccountingAction();

// добавляем автомобили
carShop.AddPedalCar(pedalSize: 2);
carShop.AddPedalCar(pedalSize: 3);
carShop.AddHandCar();
carShop.AddHandCar();

// Добавим лишний автомобиль
carShop.AddPedalCar(pedalSize: 4);

// отменяем последнюю команду
carShop.UndoLastAccountingAction();

// сохраняем внесенные изменения
carShop.SaveCarsAndCustomers();

// выводим список покупателей
carShop.ShowCustomers();

// продаем автомобили
carShop.SellCars();

// экспортируем отчет в формате Markdown
carShop.ExportReport(ReportFormat.Markdown, Console.Out);

// экспортируем отчет в формате JSON
using var reportFile = new StreamWriter("report.json");

carShop.ExportReport(ReportFormat.Json, reportFile);
```

Запустим приложение и проверим, что все работает корректно.

Как видим, наш код стал значительно проще и понятнее. В этом цель данного паттерна.

## Адаптер

Адаптер - это структурный паттерн проектирования, который позволяет объектам с несовместимыми интерфейсами работать вместе.

В нашем примере мы добавим адаптер выгрузки данных отчета на специальный сервер отчетов, имеющий свой собственный клиент, который ничего не знает о нашем приложении.

Файлы сервера и клиента можно найти в проектах `ReportServer` и `ReportServer.Client` соответственно.

Для реализации адаптера нам необходимо добавить новую реализацию для класса `ReportExporter`, которая не будет записывать данные в экземпляр `TextWriter`, а будет использовать HTTP-запросы к серверу отчетов.

Начнем с добавления ссылки на клиентскую библиотеку в наш проект. Для этого в файл `UniversalCarShop.csproj` добавим следующий код:

```xml
<ItemGroup>
    <ProjectReference Include="..\ReportServer.Client\ReportServer.Client.csproj" />
</ItemGroup>
```

Такой подход к добавлению ссылок в проект хорош тем, что работает независимо от используемой IDE.

Теперь добавим новый элемент в перечисление `ReportFormat`:

```csharp
public enum ReportFormat
{
    Markdown,
    Json,
    Server
}
```

Теперь добавим новую реализацию для класса `ReportExporter`:

```csharp
using ReportServer.Client;

using ExternalReport = ReportServer.Client.Report;

public class ReportServerExporter : ReportExporter
{
    private readonly ReportServerClient _reportServerClient;

    public ReportServerExporter(ReportServerClient reportServerClient)
    {
        _reportServerClient = reportServerClient;
    }

    public override void Export(Report report, TextWriter writer)
    {
        var externalReport = new ExternalReport
        {
            Title = report.Title,
            Contents = report.Content
        };

        _reportServerClient.StoreReportAsync(externalReport).Wait();
    }
}
```

Модцифируем фабрику для создания экспортеров. Для начала добавим ссылку на клиент для сервера отчетов:

```csharp
private readonly ReportServerClient _reportServerClient;

public ReportExporterFactory(ReportServerClient reportServerClient)
{
    _reportServerClient = reportServerClient;
}
```

Теперь модифицируем метод `Create` для создания экспортеров:

```csharp
public ReportExporter Create(ReportFormat format)
{
    return format switch
    {
        ReportFormat.Json => new JsonReportExporter(),
        ReportFormat.Markdown => new MarkdownReportExporter(),
        ReportFormat.Server => new ReportServerExporter(_reportServerClient),
        _ => throw new ArgumentException($"Неизвестный формат экспорта: {format}")
    };
}
```

Но если мы прямо сейчас попытаемся использовать новый формат экспорта, то получим ошибку, так как наш контейнер зависимостей не знает, как создать экземпляр `ReportServerClient`.

Для того чтобы решить эту проблему, нам необходимо добавить в контейнер зависимость `ReportServerClient`. Для этого в методе `CreateServiceProvider` класса `CompositionRoot` добавим следующий код:

```csharp
services.AddSingleton(sp => new ReportServerClient("http://localhost:5000"));
```

Теперь в `Program.cs` мы можем использовать новый формат экспорта:

```csharp
carShop.ExportReport(ReportFormat.Server, Console.Out);
```

Запустим сервер отчетов следующей командой:

```bash
dotnet run --project ./ReportServer
```

Теперь запустим наше приложение и проверим, что все работает корректно.

```bash
dotnet run --project ./UniversalCarShop/UniversalCarShop.csproj
```



