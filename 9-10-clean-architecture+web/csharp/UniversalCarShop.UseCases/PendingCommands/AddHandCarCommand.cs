using System;
using UniversalCarShop.UseCases.Cars;
using UniversalCarShop.UseCases.Engines;

namespace UniversalCarShop.UseCases.PendingCommands;

/// <summary>
/// Класс команды добавления автомобиля с ручным приводом.
/// Реализует интерфейс <see cref="IAccountingSessionCommand"/>.
/// </summary>
internal sealed class AddHandCarCommand(
    ICarRepository carRepository,
    ICarNumberService carNumberService,
    ICarFactory<HandEngineParams> handCarFactory) : IAccountingSessionCommand
{
    /// <summary>
    /// Метод применения команды.
    /// Добавляет автомобиль с ручным приводом в сервис автомобилей.
    /// </summary>
    public void Apply()
    {
        var number = carNumberService.GetNextNumber();
        var car = handCarFactory.CreateCar(HandEngineParams.DEFAULT, number);
        carRepository.Add(car);
    }

    /// <summary>
    /// Метод применения команды.
    /// Добавляет автомобиль с ручным приводом в сервис автомобилей.
    /// </summary>
    public override string ToString() => "Добавлен автомобиль с ручным приводом.";
}
