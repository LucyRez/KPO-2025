using UniversalCarShop.Customers;

namespace UniversalCarShop.Domain;

public sealed record CustomerAddedEvent(Customer Customer, DateTime OccurredOn) : IDomainEvent;