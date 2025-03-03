using Microsoft.Extensions.DependencyInjection;
using UniversalCarShop;
using UniversalCarShop.Accounting;
using UniversalCarShop.Cars;
using UniversalCarShop.Customers;
using UniversalCarShop.Engines;
using UniversalCarShop.Reports;

var services = CompositionRoot.Services;

var customers = services.GetRequiredService<CustomersStorage>();
var cars = services.GetRequiredService<CarService>();
var pedalCarFactory = services.GetRequiredService<PedalCarFactory>();
var handCarFactory = services.GetRequiredService<HandCarFactory>();

var hse = services.GetRequiredService<HseCarService>();
var report = new ReportBuilder();
var accountingSession = new AccountingSession(report); // создаем сеанс работы с системой учета

// добавляем покупателей

accountingSession.AddCommand(new AddCustomerCommand(
    customersStorage: customers,
    name: "Ваня",
    legPower: 6,
    handPower: 4
));

accountingSession.AddCommand(new AddCustomerCommand(
    customersStorage: customers,
    name: "Света",
    legPower: 4,
    handPower: 6
));

accountingSession.AddCommand(new AddCustomerCommand(
    customersStorage: customers,
    name: "Сергей",
    legPower: 6,
    handPower: 6
));

accountingSession.AddCommand(new AddCustomerCommand(
    customersStorage: customers,
    name: "Алексей",
    legPower: 4,
    handPower: 4
));

// Добавим лишнего покупателя

accountingSession.AddCommand(new AddCustomerCommand(
    customersStorage: customers,
    name: "Маша",
    legPower: 4,
    handPower: 4
));

// отменяем последнюю команду
accountingSession.UndoLastCommand();

// добавляем автомобили
accountingSession.AddCommand(new AddPedalCarCommand(pedalCarFactory, cars, 2)); // добавляем педальный автомобиль
accountingSession.AddCommand(new AddPedalCarCommand(pedalCarFactory, cars, 3)); // добавляем педальный автомобиль
accountingSession.AddCommand(new AddHandCarCommand(handCarFactory, cars)); // добавляем автомобиль с ручным приводом
accountingSession.AddCommand(new AddHandCarCommand(handCarFactory, cars)); // добавляем автомобиль с ручным приводом

// Добавим лишний автомобиль
accountingSession.AddCommand(new AddPedalCarCommand(pedalCarFactory, cars, 4)); // добавляем педальный автомобиль

// отменяем последнюю команду
accountingSession.UndoLastCommand();

// сохраняем внесенные изменения
accountingSession.SaveChanges();

report
    .AddOperation("Вывод списка покупателей")
    .AddCustomers(customers.GetCustomers());

hse.SellCars();

report
    .AddOperation("Продажа автомобилей")
    .AddCustomers(customers.GetCustomers());

// создаем экспортер в формате Markdown
var markdownExporter = services
    .GetRequiredService<ReportExporterFactory>()
    .Create(ReportFormat.Markdown);

markdownExporter.Export(report.Build(), Console.Out); // экспортируем отчет в консоль

// создаем экспортер в формате JSON
var jsonExporter = services
    .GetRequiredService<ReportExporterFactory>()
    .Create(ReportFormat.Json);

using var reportFile = new StreamWriter("report.json"); // создаем файл для записи отчета

jsonExporter.Export(report.Build(), reportFile); // экспортируем отчет в файл