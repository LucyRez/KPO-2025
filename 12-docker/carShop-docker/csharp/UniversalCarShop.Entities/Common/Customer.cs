using System.Text;

namespace UniversalCarShop.Entities.Common;

/// <summary>
/// Покупатель
/// </summary>
public class Customer
{
    public Customer(string name, CustomerCapabilities capabilities) : this(name, capabilities, car: null)
    {

    }

    public Customer(string name, CustomerCapabilities capabilities, Car? car)
    {
        Name = name;
        Capabilities = capabilities;
        Car = car;
    }
    
    /// <summary>
    /// Имя покупателя
    /// </summary>
    public string Name
    {
        get; // имя не получится изменить
    }

    /// <summary>
    /// Возможности покупателя
    /// </summary>
    public CustomerCapabilities Capabilities
    {
        get; // свойство только для чтения
    }

    /// <summary>
    /// Автомобиль
    /// </summary>
    public Car? Car { get; }

    /// <summary>
    /// Добавляет автомобиль покупателю
    /// </summary>
    public Customer AssignCar(Car car)
    {
        return new Customer(Name, Capabilities, car);
    }

    // Переопределим метод ToString для получения информации о покупателе
    public override string ToString()
    {
        var builder = new StringBuilder();

        builder.Append($"Имя: {Name}. Сила ног: {Capabilities.LegPower}. Сила рук: {Capabilities.HandPower}. ");

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