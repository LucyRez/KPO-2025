using System;
using UniversalCarShop.Cars;
using UniversalCarShop.Engines;

namespace UniversalCarShop.Accounting;

/// <summary>
/// Класс команды добавления автомобиля с ручным приводом.
/// Реализует интерфейс <see cref="IAccountingSessionCommand"/>.
/// </summary>
public sealed class AddHandCarCommand(
    ICarRepository carRepository,
    CarNumberService carNumberService,
    HandCarFactory handCarFactory) : IAccountingSessionCommand
{
    /// <summary>
    /// Метод применения команды.
    /// Добавляет автомобиль с ручным приводом в сервис автомобилей.
    /// </summary>
    public void Apply()
    {
        var number = carNumberService.GetNextNumber();
        var car = handCarFactory.CreateCar(EmptyEngineParams.DEFAULT, number);
        carRepository.Add(car);
    }

    /// <summary>
    /// Метод применения команды.
    /// Добавляет автомобиль с ручным приводом в сервис автомобилей.
    /// </summary>
    public override string ToString() => "Добавлен автомобиль с ручным приводом.";
}
