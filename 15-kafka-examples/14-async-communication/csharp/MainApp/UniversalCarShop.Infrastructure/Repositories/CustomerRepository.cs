using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using UniversalCarShop.Entities.Common;
using UniversalCarShop.Entities.Events;
using UniversalCarShop.Infrastructure.Database;
using UniversalCarShop.Infrastructure.Mappings;
using UniversalCarShop.UseCases.Customers;
using UniversalCarShop.UseCases.Events;

namespace UniversalCarShop.Infrastructure.Repositories;

/// <summary>
/// Хранилище покупателей
/// </summary>
internal sealed class CustomerRepository : ICustomerRepository
{
    private readonly IDomainEventService _domainEventService;
    private readonly AppDbContext _dbContext;

    public CustomerRepository(IDomainEventService domainEventService, AppDbContext dbContext)
    {
        _domainEventService = domainEventService;
        _dbContext = dbContext;
    }

    /// <summary>
    /// Получает всех покупателей
    /// </summary>
    public IEnumerable<Customer> GetAll() => 
        _dbContext.Customers
            .Include(c => c.Car)
            .ToList()
            .Select(c => c.ToDomain());

    /// <summary>
    /// Получает покупателя по имени
    /// </summary>
    public Customer? GetByName(string name) => _dbContext.Customers
        .Include(c => c.Car)
        .FirstOrDefault(c => c.Name == name)?
        .ToDomain();

    /// <summary>
    /// Добавляет покупателя
    /// </summary>
    public void Add(Customer customer)
    {
        _dbContext.Customers.Add(customer.ToEntity());
        _dbContext.SaveChanges();

        _domainEventService.Raise(new CustomerAddedEvent(customer, DateTime.UtcNow));
    }

    /// <summary>
    /// Добавляет автомобиль покупателю
    /// </summary>
    public void AssignCar(Customer customer, Car car)
    {
        _dbContext.Customers
            .Where(c => c.Name == customer.Name)
            .ExecuteUpdate(c => c.SetProperty(c => c.CarNumber, car.Number));
    }
}
