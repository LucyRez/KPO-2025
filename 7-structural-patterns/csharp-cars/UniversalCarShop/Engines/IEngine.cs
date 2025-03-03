using System;
using UniversalCarShop.Customers;

namespace UniversalCarShop.Engines;

/// <summary>
/// Интерфейс для описания двигателя
/// </summary>
public interface IEngine
{
    /// <summary>
    /// Метод для проверки совместимости двигателя с покупателем
    /// </summary>
    bool IsCompatible(Customer customer);
}
