using System;
using UniversalCarShop.UseCases.Cars;
using UniversalCarShop.UseCases.Engines;

namespace UniversalCarShop.UseCases.PendingCommands;

/// <summary>
/// Класс команды добавления педального автомобиля.
/// Реализует интерфейс <see cref="IAccountingSessionCommand"/>.
/// </summary>
internal sealed class AddPedalCarCommand(
    ICarRepository carRepository,
    ICarNumberService carNumberService,
    ICarFactory<PedalEngineParams> pedalCarFactory,
    int pedalSize) : IAccountingSessionCommand
{
    /// <summary>
    /// Метод применения команды.
    /// Добавляет педальный автомобиль в сервис автомобилей.
    /// </summary>
    public void Apply()
    {
        var number = carNumberService.GetNextNumber();
        var carParams = new PedalEngineParams(pedalSize);
        var car = pedalCarFactory.CreateCar(carParams, number);
        carRepository.Add(car);
    }

    /// <summary>
    /// Переопределение метода ToString для получения строкового представления команды.
    /// </summary>
    public override string ToString() => $"Добавлен педальный автомобиль. Размер педалей: {pedalSize}.";
}
