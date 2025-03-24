using System;
using UniversalCarShop.Entities.Common;
using UniversalCarShop.Entities.Events;
using UniversalCarShop.UseCases.Customers;
using UniversalCarShop.UseCases.Events;

namespace UniversalCarShop.Infrastructure.Repositories;

/// <summary>
/// Хранилище покупателей
/// </summary>
internal sealed class CustomerRepository : ICustomerRepository
{
    private readonly List<Customer> _customers = new();
    private readonly IDomainEventService _domainEventService;

    public CustomerRepository(IDomainEventService domainEventService)
    {
        _domainEventService = domainEventService;
    }

    /// <summary>
    /// Получает всех покупателей
    /// </summary>
    public IEnumerable<Customer> GetAll() => _customers;

    /// <summary>
    /// Получает покупателя по имени
    /// </summary>
    public Customer? GetByName(string name) => _customers.FirstOrDefault(c => c.Name == name);

    /// <summary>
    /// Добавляет покупателя
    /// </summary>
    public void Add(Customer customer)
    {
        _customers.Add(customer);
        _domainEventService.Raise(new CustomerAddedEvent(customer, DateTime.UtcNow));
    }
}
