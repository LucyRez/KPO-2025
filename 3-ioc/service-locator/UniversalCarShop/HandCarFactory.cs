using System;

namespace UniversalCarShop;

/// <summary>
/// Реализация фабрики для создания автомобилей с ручным приводом
/// </summary>
public class HandCarFactory : ICarFactory<EmptyEngineParams>
{
    /// <summary>
    /// Реализация метода для построения автомобилей с ручным приводом
    /// </summary>
    public Car CreateCar(EmptyEngineParams carParams, int carNumber)
    {
        var engine = new HandEngine(); // Создаем двигатель без каких-либо параметров

        return new Car(engine, carNumber); // создаем автомобиль с ручным приводом
    }
}
