using System;

namespace UniversalCarShop;

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
