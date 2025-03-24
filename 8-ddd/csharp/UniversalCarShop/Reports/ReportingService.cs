using UniversalCarShop.Customers;
using UniversalCarShop.Cars;
using UniversalCarShop.Domain;

namespace UniversalCarShop.Reports;
/// <summary>
/// Сервис для создания и экспорта отчетов
/// </summary>
public sealed class ReportingService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICarRepository _carRepository;
    private readonly ReportBuilder _reportBuilder;
    private readonly ReportExporterFactory _reportExporterFactory;
    private readonly IDomainEventService _domainEventService;

    /// <summary>
    /// Конструктор сервиса для отчетов
    /// </summary>
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

    /// <summary>
    /// Получение текущего отчета
    /// </summary>
    public Report GetCurrentReport()
    {
        return _reportBuilder.Build();
    }

    /// <summary>
    /// Экспорт отчета в различных форматах
    /// </summary>
    public void ExportReport(ReportFormat format, TextWriter writer)
    {
        if (writer == null)
            throw new ArgumentNullException(nameof(writer));
            
        var report = GetCurrentReport();
        
        var exporter = _reportExporterFactory.Create(format);
        exporter.Export(report, writer);
    }

    /// <summary>
    /// Обработка доменных событий
    /// </summary>
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