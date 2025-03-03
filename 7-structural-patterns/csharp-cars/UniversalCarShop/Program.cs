using Microsoft.Extensions.DependencyInjection;
using UniversalCarShop;
using UniversalCarShop.Accounting;
using UniversalCarShop.Cars;
using UniversalCarShop.Customers;
using UniversalCarShop.Engines;
using UniversalCarShop.Reports;

var services = CompositionRoot.Services;

var carShop = services.GetRequiredService<CarShop>();

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
carShop.AddPedalCar(pedalSize: 2); // добавляем педальный автомобиль
carShop.AddPedalCar(pedalSize: 3); // добавляем педальный автомобиль
carShop.AddHandCar(); // добавляем автомобиль с ручным приводом
carShop.AddHandCar(); // добавляем автомобиль с ручным приводом

// Добавим лишний автомобиль
carShop.AddPedalCar(pedalSize: 4); // добавляем педальный автомобиль

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

// экспортируем отчет в формате Server
carShop.ExportReport(ReportFormat.Server, Console.Out);
