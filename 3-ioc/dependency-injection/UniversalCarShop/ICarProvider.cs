using System;

namespace UniversalCarShop;

/// <summary>
/// Интерфейс, предоставляющий автомобили для покупателей
/// </summary>
public interface ICarProvider
{
    /// <summary>
    /// Метод, предоставляющий автомобили для покупателей
    /// </summary>
    Car? TakeCar(Customer customer); // Метод возвращает nullable ссылку на Car, что означает, что метод может ничего не вернуть
}
