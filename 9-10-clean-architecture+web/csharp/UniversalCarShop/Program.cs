// Демонстрация работы с покупателями
using UniversalCarShop;
using UniversalCarShop.Reports;

CompositionRoot.CustomerService.AddCustomerPending("Ваня", legPower: 6, handPower: 4);
CompositionRoot.CustomerService.AddCustomerPending("Света", legPower: 4, handPower: 6);
CompositionRoot.CustomerService.AddCustomerPending("Сергей", legPower: 6, handPower: 6);
CompositionRoot.CustomerService.AddCustomerPending("Алексей", legPower: 4, handPower: 4);

// Добавим лишнего покупателя и затем отменим действие
CompositionRoot.CustomerService.AddCustomerPending("Маша", legPower: 4, handPower: 4);
CompositionRoot.PendingCommandService.UndoLastCommand();

// Демонстрация работы с автомобилями
CompositionRoot.CarInventoryService.AddPedalCarPending(2);
CompositionRoot.CarInventoryService.AddPedalCarPending(3);
CompositionRoot.CarInventoryService.AddHandCarPending();
CompositionRoot.CarInventoryService.AddHandCarPending();

// Добавим лишний автомобиль и затем отменим действие
CompositionRoot.CarInventoryService.AddPedalCarPending(4);
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