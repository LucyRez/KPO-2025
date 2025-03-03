namespace UniversalCarShop;

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
    public int Number
    {
        get; // Номер не получится поменять после выпуска автомобиля
    }

    /// <summary>
    /// Метод для определения совместимости покупателей с автомобилями
    /// </summary>
    public bool IsCompatible(Customer customer) => _engine.IsCompatible(customer); // внутри метода просто вызываем соответствующий метод двигателя

    /// <summary>
    /// Переопределение метода ToString для получения информации об автомобиле
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"Номер: {Number}. Двигатель: {_engine}";
}