using UniversalCarShop.UseCases.Events;
using UniversalCarShop.Entities.Events;

namespace UniversalCarShop.UseCases.Reports;

/// <summary>
/// Сервис для создания и экспорта отчетов
/// </summary>
internal sealed class ReportingService
{
    private readonly IDomainEventService _domainEventService;
    private readonly IReportServerConnector _reportServerConnector;

    /// <summary>
    /// Конструктор сервиса для отчетов
    /// </summary>
    public ReportingService(IDomainEventService domainEventService, IReportServerConnector reportServerConnector)
    {
        _domainEventService = domainEventService;
        _reportServerConnector = reportServerConnector;
    }

    /// <summary>
    /// Инициализация сервиса
    /// </summary>
    public void Initialize()
    {
        _domainEventService.OnDomainEvent += HandleDomainEvent;
    }

    /// <summary>
    /// Обработка доменных событий
    /// </summary>
    private void HandleDomainEvent(IDomainEvent domainEvent)
    {
        var reportedEventDto = CreateReportedEventDto(domainEvent);

        _reportServerConnector.SendEvent(reportedEventDto);
    }

    private static ReportedEventDto CreateReportedEventDto(IDomainEvent domainEvent) => domainEvent switch
    {
        CarSoldEvent carSoldEvent => new ReportedEventDto(
            "CarSold",
            $"Автомобиль {carSoldEvent.Car.Number} продан покупателю {carSoldEvent.Customer.Name} " +
            $"(сила ног: {carSoldEvent.Customer.Capabilities.LegPower}, сила рук: {carSoldEvent.Customer.Capabilities.HandPower}) " +
            $"({carSoldEvent.OccurredOn})",
            carSoldEvent.OccurredOn
        ),
        CustomerAddedEvent customerAddedEvent => new ReportedEventDto(
            "CustomerAdded",
            $"Новый покупатель: {customerAddedEvent.Customer.Name} " +
            $"(сила ног: {customerAddedEvent.Customer.Capabilities.LegPower}, сила рук: {customerAddedEvent.Customer.Capabilities.HandPower}) " +
            $"({customerAddedEvent.OccurredOn})",
            customerAddedEvent.OccurredOn
        ),
        CarAddedEvent carAddedEvent => new ReportedEventDto(
            "CarAdded",
            $"Новый автомобиль {carAddedEvent.Car.Number}. Тип двигателя: {carAddedEvent.Car.Engine.Specification.Type} ({carAddedEvent.OccurredOn})",
            carAddedEvent.OccurredOn
        ),
        _ => throw new ArgumentException($"Неизвестное событие: {domainEvent.GetType().Name}")
    };
}