using System.Text;
using UniversalCarShop.Cars;

namespace UniversalCarShop.Customers;

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
        var builder = new StringBuilder();

        builder.Append($"Имя: {Name}. Сила ног: {LegPower}. Сила рук: {HandPower}. ");

        if (Car is null)
        {
            builder.Append("Автомобиль: { Нет }");
        }
        else
        {
            builder.Append($"Автомобиль: {{ {Car} }}");
        }

        return builder.ToString();
    }
}