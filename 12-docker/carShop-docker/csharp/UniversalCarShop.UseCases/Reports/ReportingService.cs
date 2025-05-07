using UniversalCarShop.Entities.Common;
using UniversalCarShop.UseCases.Customers;
using UniversalCarShop.UseCases.Cars;
using UniversalCarShop.UseCases.Events;
using UniversalCarShop.Entities.Events;
namespace UniversalCarShop.UseCases.Reports;

/// <summary>
/// Сервис для создания и экспорта отчетов
/// </summary>
internal sealed class ReportingService : IReportingService
{
    private readonly ReportBuilder _reportBuilder;
    private readonly IReportExporterFactory _reportExporterFactory;
    private readonly IDomainEventService _domainEventService;

    /// <summary>
    /// Конструктор сервиса для отчетов
    /// </summary>
    public ReportingService(
        ReportBuilder reportBuilder,
        IReportExporterFactory reportExporterFactory,
        IDomainEventService domainEventService)
    {
        _reportBuilder = reportBuilder;
        _reportExporterFactory = reportExporterFactory;
        _domainEventService = domainEventService;
    }

    /// <summary>
    /// Инициализация сервиса
    /// </summary>
    public void Initialize()
    {
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

            _reportBuilder.AddOperation($"Новый автомобиль {car.Number}. Тип двигателя: {car.Engine.Specification.Type} ({carAddedEvent.OccurredOn})");
        }
    }
}