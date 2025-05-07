using Microsoft.Extensions.DependencyInjection;
using UniversalCarShop.UseCases.Cars;
using UniversalCarShop.UseCases.Engines;
using UniversalCarShop.UseCases.PendingCommands;

namespace UniversalCarShop.UseCases.Cars;

/// <summary>
/// Сервис для учета автомобилей
/// </summary>
internal sealed class CarInventoryService(
    IServiceScopeFactory serviceScopeFactory,
    IPendingCommandService pendingCommandService,
    ICarFactory<PedalEngineParams> pedalCarFactory,
    ICarFactory<HandEngineParams> handCarFactory) : ICarInventoryService
{
    /// <summary>
    /// Добавляет педальный автомобиль
    /// </summary>
    public void AddPedalCarPending(int pedalSize)
    {
        var command = new AddPedalCarCommand(serviceScopeFactory, pedalCarFactory, pedalSize);
        pendingCommandService.AddCommand(command);
    }

    /// <summary>
    /// Добавляет автомобиль с ручным приводом
    /// </summary>
    public void AddHandCarPending()
    {
        var command = new AddHandCarCommand(serviceScopeFactory, handCarFactory);
        pendingCommandService.AddCommand(command);
    }
}