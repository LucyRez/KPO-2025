using UniversalCarShop.Entities.Common;
using UniversalCarShop.UseCases.Events;
using UniversalCarShop.UseCases.Cars;
using UniversalCarShop.UseCases.Customers;
using UniversalCarShop.Entities.Events;
namespace UniversalCarShop.UseCases.Sales;

/// <summary>
/// Сервис для продажи автомобилей
/// </summary>
internal sealed class SalesService(ICarRepository carRepository, ICustomerRepository customerRepository, IDomainEventService domainEventService) : ISalesService
{
    /// <summary>
    /// Продажа автомобилей
    /// </summary>
    public void SellCars()
    {
        foreach (var customer in customerRepository.GetAll())
        {
            var car = carRepository.FindCompatibleCar(customer.Capabilities); // находим совместимый автомобиль для покупателя

            if (car is not null) // если автомобиль найден
            {
                SellCar(customer, car); // продаем автомобиль покупателю
            }
        }
    }

    /// <summary>
    /// Продажа автомобиля
    /// </summary>
    private bool SellCar(Customer customer, Car car)
    {
        if (!car.IsCompatible(customer.Capabilities))
            return false;
        
        car.MarkAsSold();
        customer.AssignCar(car);
        
        domainEventService.Raise(new CarSoldEvent(car, customer, DateTime.UtcNow));
        
        return true;
    }
}