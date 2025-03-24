using UniversalCarShop.Customers;
using UniversalCarShop.Engines;

namespace UniversalCarShop.Cars;

/// <summary>
/// Автомобиль
/// </summary>
public class Car
{
    /// <summary>
    /// Двигатель автомобиля
    /// </summary>
    private readonly IEngine _engine; // двигатель нельзя ни вынуть, ни поменять

    public Car(IEngine engine, int number)
    {
        Number = number;
        
        _engine = engine;
    }

    /// <summary>
    /// Номер автомобиля
    /// </summary>
    public int Number { get;}

    /// <summary>
    /// Продан ли автомобиль
    /// </summary>
    public bool IsSold { get; private set; }

    /// <summary>
    /// Спецификация двигателя
    /// </summary>
    public EngineSpecification EngineSpecification => _engine.Specification;

    /// <summary>
    /// Метод для определения совместимости покупателей с автомобилями
    /// </summary>
    public bool IsCompatible(CustomerCapabilities customerCapabilities) => _engine.IsCompatible(customerCapabilities); // внутри метода просто вызываем соответствующий метод двигателя

    /// <summary>
    /// Продажа автомобиля
    /// </summary>
    public void MarkAsSold() => IsSold = true;

    /// <summary>
    /// Переопределение метода ToString для получения информации об автомобиле
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"Номер: {Number}. Двигатель: {_engine}";
}