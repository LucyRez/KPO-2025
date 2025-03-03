using UniversalCarShop.Accounting;
using UniversalCarShop.Cars;
using UniversalCarShop.Customers;
using UniversalCarShop.Reports;

namespace UniversalCarShop;

public sealed class CarShop
{
    private readonly CustomersStorage _customersStorage;
    private readonly CarService _carService;
    private readonly PedalCarFactory _pedalCarFactory;
    private readonly HandCarFactory _handCarFactory;
    private readonly HseCarService _hseCarService;
    private readonly ReportBuilder _reportBuilder;
    private readonly AccountingSession _accountingSession;
    private readonly ReportExporterFactory _reportExporterFactory;

    public CarShop(
        CustomersStorage customersStorage,
        CarService carService,
        PedalCarFactory pedalCarFactory,
        HandCarFactory handCarFactory,
        HseCarService hseCarService,
        ReportExporterFactory reportExporterFactory
    ) {
        _customersStorage = customersStorage;
        _carService = carService;
        _pedalCarFactory = pedalCarFactory;
        _handCarFactory = handCarFactory;
        _hseCarService = hseCarService;
        _reportBuilder = new ReportBuilder();
        _accountingSession = new AccountingSession(_reportBuilder);
        _reportExporterFactory = reportExporterFactory;
    }

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

    public void ExportReport(ReportFormat format, TextWriter writer)
    {
        var exporter = _reportExporterFactory.Create(format);

        exporter.Export(_reportBuilder.Build(), writer);
    }
}

