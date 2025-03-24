using System;

namespace UniversalCarShop.Accounting;

/// <summary>
/// Интерфейс команды сеанса учета.
/// </summary>
public interface IAccountingSessionCommand
{
    /// <summary>
    /// Применить команду.
    /// </summary>
    void Apply();
}
