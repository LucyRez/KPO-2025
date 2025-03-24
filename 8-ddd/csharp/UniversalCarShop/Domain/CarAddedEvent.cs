using UniversalCarShop.Cars;

namespace UniversalCarShop.Domain;

public sealed record CarAddedEvent(Car Car, DateTime OccurredOn) : IDomainEvent;
