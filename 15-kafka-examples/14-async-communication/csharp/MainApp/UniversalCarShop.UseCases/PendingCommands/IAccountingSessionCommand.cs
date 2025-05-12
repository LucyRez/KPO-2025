using System;

namespace UniversalCarShop.UseCases.PendingCommands;

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
