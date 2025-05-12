using System;
using UniversalCarShop.Entities.Common;

namespace UniversalCarShop.Entities.Common;

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
