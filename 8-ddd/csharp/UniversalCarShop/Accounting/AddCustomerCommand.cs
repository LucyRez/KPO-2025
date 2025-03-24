using System;
using UniversalCarShop.Customers;

namespace UniversalCarShop.Accounting;

/// <summary>
/// Класс команды добавления покупателя.
/// Реализует интерфейс IAccountingSessionCommand.
/// </summary>
public sealed class AddCustomerCommand(
    ICustomerRepository customerRepository,
    string name,
    int legPower,
    int handPower) : IAccountingSessionCommand
{
    private readonly CustomerCapabilities _capabilities = new(legPower, handPower);

    /// <summary>
    /// Метод применения команды.
    /// Добавляет покупателя в хранилище.
    /// </summary>
    public void Apply()
    {
        customerRepository.Add(new Customer(name, _capabilities));
    }

    /// <summary>
    /// Переопределение метода ToString для получения строкового представления команды.
    /// Возвращает строку с информацией о добавленном покупателе.
    /// </summary>
    /// <returns>Строковое представление команды.</returns>
    public override string ToString() => $"Добавлен покупатель {name}. Сила ног: {legPower}. Сила рук: {handPower}.";
}
