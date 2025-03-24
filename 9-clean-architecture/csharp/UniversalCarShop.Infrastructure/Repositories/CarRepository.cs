using System;
using UniversalCarShop.Entities.Common;
using UniversalCarShop.Entities.Events;
using UniversalCarShop.UseCases.Cars;
using UniversalCarShop.UseCases.Events;

namespace UniversalCarShop.Infrastructure.Repositories;

/// <summary>
/// Репозиторий автомобилей
/// </summary>
internal sealed class CarRepository : ICarRepository
{
    private readonly List<Car> _cars = new();
    private readonly IDomainEventService _domainEventService;

    public CarRepository(IDomainEventService domainEventService)
    {
        _domainEventService = domainEventService;
    }

    public IEnumerable<Car> GetAll() => _cars.AsReadOnly();

    public void Add(Car car)
    {
        _cars.Add(car);
        _domainEventService.Raise(new CarAddedEvent(car, DateTime.UtcNow));
    }

    public Car? FindCompatibleCar(CustomerCapabilities capabilities) => _cars
        .FirstOrDefault(car => !car.IsSold && car.IsCompatible(capabilities));
}
