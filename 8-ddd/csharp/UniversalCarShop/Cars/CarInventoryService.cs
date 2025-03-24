using UniversalCarShop.Accounting;

namespace UniversalCarShop.Cars;

/// <summary>
/// Сервис для учета автомобилей
/// </summary>
public sealed class CarInventoryService(
    PendingCommandService pendingCommandService,
    ICarRepository carRepository,
    CarNumberService carNumberService,
    PedalCarFactory pedalCarFactory,
    HandCarFactory handCarFactory)
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