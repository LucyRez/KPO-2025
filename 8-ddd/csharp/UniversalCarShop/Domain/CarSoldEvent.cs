using UniversalCarShop.Customers;
using UniversalCarShop.Cars;

namespace UniversalCarShop.Domain;

/// <summary>
/// Событие продажи автомобиля
/// </summary>
public sealed record CarSoldEvent(Car Car, Customer Customer, DateTime OccurredOn) : IDomainEvent;