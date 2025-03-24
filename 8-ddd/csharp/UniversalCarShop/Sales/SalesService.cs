using UniversalCarShop.Domain;
using UniversalCarShop.Customers;
using UniversalCarShop.Cars;

namespace UniversalCarShop.Sales;

/// <summary>
/// Сервис для продажи автомобилей
/// </summary>
public sealed class SalesService(ICarRepository carRepository, ICustomerRepository customerRepository, IDomainEventService domainEventService)
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
    public bool SellCar(Customer customer, Car car)
    {
        if (!car.IsCompatible(customer.Capabilities))
            return false;
        
        car.MarkAsSold();
        customer.AssignCar(car);
        
        domainEventService.Raise(new CarSoldEvent(car, customer, DateTime.UtcNow));
        
        return true;
    }
}