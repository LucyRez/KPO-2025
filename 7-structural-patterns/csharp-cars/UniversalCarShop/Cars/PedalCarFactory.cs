using UniversalCarShop.Engines;

namespace UniversalCarShop.Cars;

/// <summary>
/// Реализация фабрики для создания педальных автомобилей
/// </summary>
public class PedalCarFactory : ICarFactory<PedalEngineParams>
{
    /// <summary>
    /// Реализация метода для создания педальных автомобилей
    /// </summary>
    public Car CreateCar(PedalEngineParams carParams, int carNumber)
    {
        var engine = new PedalEngine(carParams.PedalSize); // создаем двигатель на основе переданных параметров

        return new Car(engine, carNumber); // возвращаем собранный автомобиль с установленным двигателем и определенным номером
    }
}
