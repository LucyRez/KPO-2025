using System;
using UniversalCarShop.Cars;
using UniversalCarShop.Engines;

namespace UniversalCarShop.Accounting;

/// <summary>
/// Класс команды добавления автомобиля с ручным приводом.
/// Реализует интерфейс <see cref="IAccountingSessionCommand"/>.
/// </summary>
public sealed class AddHandCarCommand : IAccountingSessionCommand
{
    // Фабрика для создания автомобилей с ручным приводом.
    private readonly HandCarFactory _handCarFactory;
    // Сервис для добавления автомобилей.
    private readonly CarService _carService;

    /// <summary>
    /// Конструктор класса.
    /// </summary>
    /// <param name="handCarFactory">Фабрика для создания автомобилей с ручным приводом.</param>
    /// <param name="carService">Сервис для добавления автомобилей.</param>
    public AddHandCarCommand(HandCarFactory handCarFactory, CarService carService)
    {
        // Устанавливаем фабрику автомобилей.
        _handCarFactory = handCarFactory;
        // Устанавливаем сервис автомобилей.
        _carService = carService;
    }

    /// <summary>
    /// Метод применения команды.
    /// Добавляет автомобиль с ручным приводом в сервис автомобилей.
    /// </summary>
    public void Apply()
    {
        // Добавляем автомобиль с ручным приводом, используя фабрику и пустые параметры двигателя.
        _carService.AddCar(_handCarFactory, EmptyEngineParams.DEFAULT);
    }

    /// <summary>
    /// Переопределение метода ToString для получения строкового представления команды.
    /// </summary>
    /// <returns>Строка, описывающая команду добавления автомобиля с ручным приводом.</returns>
    public override string ToString() => "Добавлен автомобиль с ручным приводом.";
}
