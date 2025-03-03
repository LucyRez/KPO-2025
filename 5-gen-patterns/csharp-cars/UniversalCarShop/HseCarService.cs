using System;

namespace UniversalCarShop;

/// <summary>
/// 
/// </summary>

public class HseCarService
{
    /// <summary>
    /// Ссылка на сервис, предоставляющий нам автомобили
    /// </summary>
    private readonly ICarProvider _carProvider;

    /// <summary>
    /// Ссылка на сервис, предоставляющий нам покупателей
    /// </summary>
    private readonly ICustomersProvider _customersProvider;

    /// <summary>
    /// Конструктор класса
    /// </summary>
    public HseCarService(ICarProvider carProvider, ICustomersProvider customersProvider)
    {
        _carProvider = carProvider;
        _customersProvider = customersProvider;
    }

    public void SellCars()
    {
        // получаем список покупателей
        var customers = _customersProvider.GetCustomers();

        // пробегаемся по полученному списку
        foreach (var customer in customers)
        {
            if (customer.Car is not null)
            {
                continue; // если у покупателя уже есть автомобиль - пропускаем его
            }

            // если у покупателя нет автомобиля - подберем для него автомобиль
            var car = _carProvider.TakeCar(customer);

            if (car is null)
            {
                continue; // если не удалось получить автомобиль, то покупателю не повезло - пропускаем его
            }

            customer.Car = car; // иначе вручаем автомобиль
        }
    }
}
