using UniversalCarShop.UseCases.Cars;
using UniversalCarShop.UseCases.Engines;
using UniversalCarShop.UseCases.PendingCommands;

namespace UniversalCarShop.UseCases.Cars;

/// <summary>
/// Сервис для учета автомобилей
/// </summary>
internal sealed class CarInventoryService(
    IPendingCommandService pendingCommandService,
    ICarRepository carRepository,
    ICarNumberService carNumberService,
    ICarFactory<PedalEngineParams> pedalCarFactory,
    ICarFactory<HandEngineParams> handCarFactory) : ICarInventoryService
{
    /// <summary>
    /// Добавляет педальный автомобиль
    /// </summary>
    public void AddPedalCarPending(int pedalSize)
    {
        var command = new AddPedalCarCommand(carRepository, carNumberService, pedalCarFactory, pedalSize);
        pendingCommandService.AddCommand(command);
    }

    /// <summary>
    /// Добавляет автомобиль с ручным приводом
    /// </summary>
    public void AddHandCarPending()
    {
        var command = new AddHandCarCommand(carRepository, carNumberService, handCarFactory);
        pendingCommandService.AddCommand(command);
    }
}