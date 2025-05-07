using UniversalCarShop.Entities.Events;

namespace UniversalCarShop.UseCases.Events;

internal sealed class DomainEventService : IDomainEventService
{
    public void Raise(IDomainEvent domainEvent)
    {
        OnDomainEvent?.Invoke(domainEvent);
    }

    public event Action<IDomainEvent>? OnDomainEvent;
}
