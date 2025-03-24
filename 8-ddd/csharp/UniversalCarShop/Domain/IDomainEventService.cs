namespace UniversalCarShop.Domain;

public interface IDomainEventService
{
    void Raise(IDomainEvent domainEvent);
    event Action<IDomainEvent> OnDomainEvent;
}
