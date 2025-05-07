using UniversalCarShop.Entities.Events;

namespace UniversalCarShop.UseCases.Events;

public interface IDomainEventService
{
    void Raise(IDomainEvent domainEvent);
    event Action<IDomainEvent> OnDomainEvent;
}
