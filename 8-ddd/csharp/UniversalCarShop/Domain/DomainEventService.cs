namespace UniversalCarShop.Domain;

public sealed class DomainEventService : IDomainEventService
{
    public void Raise(IDomainEvent domainEvent)
    {
        OnDomainEvent?.Invoke(domainEvent);
    }

    public event Action<IDomainEvent>? OnDomainEvent;
}
