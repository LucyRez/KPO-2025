using System;
using Microsoft.Extensions.DependencyInjection;
using UniversalCarShop.Entities.Common;
using UniversalCarShop.UseCases.Customers;

namespace UniversalCarShop.UseCases.PendingCommands;

/// <summary>
/// Класс команды добавления покупателя.
/// Реализует интерфейс IAccountingSessionCommand.
/// </summary>
internal sealed class AddCustomerCommand(
    IServiceScopeFactory serviceScopeFactory,
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
        using var scope = serviceScopeFactory.CreateScope();
        var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
        customerRepository.Add(new Customer(name, _capabilities));
    }

    /// <summary>
    /// Переопределение метода ToString для получения строкового представления команды.
    /// Возвращает строку с информацией о добавленном покупателе.
    /// </summary>
    /// <returns>Строковое представление команды.</returns>
    public override string ToString() => $"Добавлен покупатель {name}. Сила ног: {legPower}. Сила рук: {handPower}.";
}
