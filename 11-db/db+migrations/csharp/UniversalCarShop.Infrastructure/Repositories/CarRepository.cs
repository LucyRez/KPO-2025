using System;
using UniversalCarShop.Entities.Common;
using UniversalCarShop.Entities.Events;
using UniversalCarShop.Infrastructure.Database;
using UniversalCarShop.Infrastructure.Mappings;
using UniversalCarShop.UseCases.Cars;
using UniversalCarShop.UseCases.Events;

namespace UniversalCarShop.Infrastructure.Repositories;

/// <summary>
/// Репозиторий автомобилей
/// </summary>
internal sealed class CarRepository : ICarRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IDomainEventService _domainEventService;

    public CarRepository(IDomainEventService domainEventService, AppDbContext dbContext)
    {
        _domainEventService = domainEventService;
        _dbContext = dbContext;
    }

    public IEnumerable<Car> GetAll() => _dbContext.Cars.AsEnumerable().Select(c => c.ToDomain());

    public void Add(Car car)
    {
        _dbContext.Cars.Add(car.ToEntity());
        _dbContext.SaveChanges();

        _domainEventService.Raise(new CarAddedEvent(car, DateTime.UtcNow));
    }

    public Car? FindCompatibleCar(CustomerCapabilities capabilities)
    {
        var query = (
            from car in _dbContext.Cars
            join customer in _dbContext.Customers on car.Number equals customer.CarNumber into customers
            from customer in customers.DefaultIfEmpty()
            where customer == null
            select car
        );

        return query
            .AsEnumerable()
            .Select(c => c.ToDomain())
            .FirstOrDefault(c => c.IsCompatible(capabilities));
    }
}
