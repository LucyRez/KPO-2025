using System;
using UniversalCarShop.Cars;
using UniversalCarShop.Engines;

namespace UniversalCarShop.Accounting;

/// <summary>
/// Класс команды добавления педального автомобиля.
/// Реализует интерфейс <see cref="IAccountingSessionCommand"/>.
/// </summary>
public sealed class AddPedalCarCommand(
    ICarRepository carRepository,
    CarNumberService carNumberService,
    PedalCarFactory pedalCarFactory,
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
