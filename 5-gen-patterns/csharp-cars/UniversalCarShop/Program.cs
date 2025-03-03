using Microsoft.Extensions.DependencyInjection;
using UniversalCarShop;

var services = CompositionRoot.Services;

var customers = services.GetRequiredService<CustomersStorage>();
var cars = services.GetRequiredService<CarService>();
var pedalCarFactory = services.GetRequiredService<PedalCarFactory>();
var handCarFactory = services.GetRequiredService<HandCarFactory>();

var hse = services.GetRequiredService<HseCarService>();

// добавляем покупателей

customers.AddCustomer(new Customer(
    name: "Ваня",
    legPower: 6,
    handPower: 4
));

customers.AddCustomer(new Customer(
    name: "Света",
    legPower: 4,
    handPower: 6
));

customers.AddCustomer(new Customer(
    name: "Сергей",
    legPower: 6,
    handPower: 6
));

customers.AddCustomer(new Customer(
    name: "Алексей",
    legPower: 4,
    handPower: 4
));

// добавляем автомобили

cars.AddCar(pedalCarFactory, new PedalEngineParams(2)); // добавляем педальный автомобиль
cars.AddCar(pedalCarFactory, new PedalEngineParams(3)); // добавляем педальный автомобиль
cars.AddCar(handCarFactory, EmptyEngineParams.DEFAULT); // добавляем автомобиль с ручным приводом
cars.AddCar(handCarFactory, EmptyEngineParams.DEFAULT); // добавляем автомобиль с ручным приводом

var report = new ReportBuilder()
    .AddOperation("Инициализация системы")
    .AddCustomers(customers.GetCustomers());

hse.SellCars();

report
    .AddOperation("Продажа автомобиля")
    .AddCustomers(customers.GetCustomers());

Console.WriteLine(report.Build());
