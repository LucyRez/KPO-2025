namespace UniversalCarShop;

/// <summary>
/// Покупатель
/// </summary>
public class Customer
{
    public Customer(string name, int legPower, int handPower)
    {
        Name = name;
        LegPower = legPower;
        HandPower = handPower;
    }
    
    /// <summary>
    /// Имя покупателя
    /// </summary>
    public string Name
    {
        get; // имя не получится изменить
    }

    /// <summary>
    /// Сила ног
    /// </summary>
    public int LegPower
    {
        get; // свойство только для чтения
    }

    /// <summary>
    /// Сила рук
    /// </summary>
    public int HandPower
    {
        get; // свойство только для чтения
    }

    /// <summary>
    /// Автомобиль
    /// </summary>
    public Car? Car // покупатель существует независимо от автомобиля - поэтому поле допускает значения null
    {
        get; // можно как узнать, какой автомобиль у покупателя
        set; // так и изменить его
    }

    // Переопределим метод ToString для получения информации о покупателе
    public override string ToString()
    {
        if (Car is null)
        {
            return $"Имя: {Name}. Автомобиль: {{ Нет }}";
        }

        return $"Имя: {Name}. Автомобиль: {{ {Car} }}";
    }
}