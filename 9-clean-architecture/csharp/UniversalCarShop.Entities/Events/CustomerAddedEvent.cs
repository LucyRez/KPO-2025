using UniversalCarShop.Entities.Common;

namespace UniversalCarShop.Entities.Events;

public sealed record CustomerAddedEvent(Customer Customer, DateTime OccurredOn) : IDomainEvent;