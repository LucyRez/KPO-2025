using UniversalCarShop.Entities.Common;

namespace UniversalCarShop.Entities.Events;

public sealed record CarAddedEvent(Car Car, DateTime OccurredOn) : IDomainEvent;
