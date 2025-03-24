using System;
using UniversalCarShop.Customers;

namespace UniversalCarShop.Engines;

/// <summary>
/// Интерфейс для описания двигателя
/// </summary>
public interface IEngine
{
    /// <summary>
    /// Спецификация двигателя
    /// </summary>
    EngineSpecification Specification { get; }

    /// <summary>
    /// Проверяет совместимость двигателя с покупателем
    /// </summary>
    bool IsCompatible(CustomerCapabilities capabilities);
}
