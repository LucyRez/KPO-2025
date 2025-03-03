using System;
using UniversalCarShop.Cars;
using UniversalCarShop.Engines;

namespace UniversalCarShop.Accounting;

/// <summary>
/// Класс команды добавления педального автомобиля.
/// Реализует интерфейс <see cref="IAccountingSessionCommand"/>.
/// </summary>
public sealed class AddPedalCarCommand : IAccountingSessionCommand
{
    // Фабрика для создания педальных автомобилей.
    private readonly PedalCarFactory _pedalCarFactory;
    // Сервис для добавления автомобилей.
    private readonly CarService _carService;
    // Размер педалей.
    private readonly int _size;

    /// <summary>
    /// Конструктор класса.
    /// </summary>
    /// <param name="pedalCarFactory">Фабрика педальных автомобилей.</param>
    /// <param name="carService">Сервис автомобилей.</param>
    /// <param name="size">Размер педалей.</param>
    public AddPedalCarCommand(PedalCarFactory pedalCarFactory, CarService carService, int size)
    {
        // Устанавливаем фабрику педальных автомобилей.
        _pedalCarFactory = pedalCarFactory;
        // Устанавливаем сервис автомобилей.
        _carService = carService;
        // Устанавливаем размер педалей.
        _size = size;
    }

    /// <summary>
    /// Метод применения команды.
    /// Добавляет педальный автомобиль в сервис автомобилей.
    /// </summary>
    public void Apply()
    {
        // Добавляем педальный автомобиль, используя фабрику и параметры двигателя.
        _carService.AddCar(_pedalCarFactory, new PedalEngineParams(_size));
    }

    /// <summary>
    /// Переопределение метода ToString для получения строкового представления команды.
    /// </summary>
    public override string ToString() => $"Добавлен педальный автомобиль. Размер педалей: {_size}.";
}
