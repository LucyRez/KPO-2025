using System;

namespace UniversalCarShop.Cars;

/// <summary>
/// Обобщенный интерфейс фабрики для создания автомобилей
/// </summary>
public interface ICarFactory<TParams> // у интерфейса есть параметр типа TParams для передачи параметров
{
    /// <summary>
    /// Метод для конструкирования автомобилей
    /// </summary>
    Car CreateCar(TParams carParams, int carNumber);
}
