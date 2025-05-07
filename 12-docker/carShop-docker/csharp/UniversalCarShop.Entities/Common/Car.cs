namespace UniversalCarShop.Entities.Common;

/// <summary>
/// Автомобиль
/// </summary>
public class Car
{
    public Car(IEngine engine, int number) : this(engine, number, isSold: false)
    {

    }

    public Car(IEngine engine, int number, bool isSold)
    {
        Number = number;
        Engine = engine;
        IsSold = isSold;
    }

    public IEngine Engine { get; }

    /// <summary>
    /// Номер автомобиля
    /// </summary>
    public int Number { get;}

    public bool IsSold { get; }

    /// <summary>
    /// Метод для определения совместимости покупателей с автомобилями
    /// </summary>
    public bool IsCompatible(CustomerCapabilities customerCapabilities) => Engine.IsCompatible(customerCapabilities); // внутри метода просто вызываем соответствующий метод двигателя

    /// <summary>
    /// Продажа автомобиля
    /// </summary>
    public Car Sell() => new(Engine, Number, isSold: true);

    /// <summary>
    /// Переопределение метода ToString для получения информации об автомобиле
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"Номер: {Number}. Двигатель: {Engine}";
}