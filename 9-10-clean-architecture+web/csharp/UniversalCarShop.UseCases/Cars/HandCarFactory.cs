using System;
using UniversalCarShop.Entities.Common;
using UniversalCarShop.UseCases.Engines;

namespace UniversalCarShop.UseCases.Cars;

/// <summary>
/// Реализация фабрики для создания автомобилей с ручным приводом
/// </summary>
internal sealed class HandCarFactory : ICarFactory<HandEngineParams>
{
    /// <summary>
    /// Реализация метода для построения автомобилей с ручным приводом
    /// </summary>
    public Car CreateCar(HandEngineParams carParams, int carNumber)
    {
        var engine = new HandEngine(); // Создаем двигатель без каких-либо параметров

        return new Car(engine, carNumber); // создаем автомобиль с ручным приводом
    }
}
