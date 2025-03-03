using Microsoft.Extensions.DependencyInjection;
using UniversalCarShop;

var services = new ServiceCollection();

services.AddSingleton<CarService>(); // регистрируем сервис для управления автомобилями
services.AddSingleton<CustomersStorage>(); // регистрируем сервис для управления покупателями
services.AddSingleton<PedalCarFactory>(); // регистрируем сервис для создания педальных автомобилей
services.AddSingleton<HandCarFactory>(); // регистрируем сервис для создания автомобилей с ручным приводом

services.AddSingleton<ICarProvider>(sp => sp.GetRequiredService<CarService>()); // регистрируем интерфейс ICarProvider в качестве сервиса
services.AddSingleton<ICustomersProvider>(sp => sp.GetRequiredService<CustomersStorage>()); // регистрируем интерфейс ICustomersProvider в качестве сервиса

services.AddSingleton<HseCarService>(); // регистрируем сервис по продаже автомобилей

var serviceProvider = services.BuildServiceProvider(); // строим контейнер зависимостей

var customers = serviceProvider.GetRequiredService<CustomersStorage>();
var cars = serviceProvider.GetRequiredService<CarService>();
var pedalCarFactory = serviceProvider.GetRequiredService<PedalCarFactory>();
var handCarFactory = serviceProvider.GetRequiredService<HandCarFactory>();
var hse = serviceProvider.GetRequiredService<HseCarService>();

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

// Выведем информацию о покупателях

Console.WriteLine("=== Покупатели ===");

foreach (var customer in customers.GetCustomers())
{
    Console.WriteLine(customer);
}

// продадим автомобили
Console.WriteLine("=== Продажа автомобилей ===");

hse.SellCars();

// Выведем информацию о покупателях

Console.WriteLine("=== Покупатели ===");

foreach (var customer in customers.GetCustomers())
{
    Console.WriteLine(customer);
}