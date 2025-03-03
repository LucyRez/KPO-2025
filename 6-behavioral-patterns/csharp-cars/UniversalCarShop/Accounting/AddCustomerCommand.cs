using System;
using UniversalCarShop.Customers;

namespace UniversalCarShop.Accounting;

/// <summary>
/// Класс команды добавления покупателя.
/// Реализует интерфейс IAccountingSessionCommand.
/// </summary>
public sealed class AddCustomerCommand : IAccountingSessionCommand
{
    /// <summary>
    /// Поле для хранения хранилища покупателей.
    /// </summary>
    private readonly CustomersStorage _customersStorage;
    private readonly string _name;
    private readonly int _legPower;
    private readonly int _handPower;

    /// <summary>
    /// Конструктор класса.
    /// Принимает хранилище покупателей и покупателя.
    /// </summary>
    /// <param name="customersStorage">Хранилище покупателей.</param>
    /// <param name="customer">Покупатель.</param>
    public AddCustomerCommand(CustomersStorage customersStorage, string name, int legPower, int handPower)
    {
        // Устанавливаем значения полей.
        _customersStorage = customersStorage;
        _name = name;
        _legPower = legPower;
        _handPower = handPower;
    }

    /// <summary>
    /// Метод применения команды.
    /// Добавляет покупателя в хранилище.
    /// </summary>
    public void Apply()
    {
        _customersStorage.AddCustomer(new Customer(_name, _legPower, _handPower));
    }

    /// <summary>
    /// Переопределение метода ToString для получения строкового представления команды.
    /// Возвращает строку с информацией о добавленном покупателе.
    /// </summary>
    /// <returns>Строковое представление команды.</returns>
    public override string ToString() => $"Добавлен покупатель {_name}. Сила ног: {_legPower}. Сила рук: {_handPower}.";
}
