using UniversalCarShop.Entities.Common;

namespace UniversalCarShop.Entities.Events;

/// <summary>
/// Событие продажи автомобиля
/// </summary>
public sealed record CarSoldEvent(Car Car, Customer Customer, DateTime OccurredOn) : IDomainEvent;