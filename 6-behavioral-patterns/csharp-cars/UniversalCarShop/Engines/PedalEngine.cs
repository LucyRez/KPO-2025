using UniversalCarShop.Customers;

namespace UniversalCarShop.Engines;

/// <summary>
/// Педальный двигатель
/// </summary>
public class PedalEngine : IEngine
{
    public PedalEngine(int size)
    {
        Size = size;
    }

    /// <summary>
    /// Размер педалей
    /// </summary>
    public int Size
    {
        get; // размер нельзя изменить после создания двигателя
    }

    /// <summary>
    /// Реализация метода определения совместимости
    /// </summary>
    public bool IsCompatible(Customer customer) => customer.LegPower > 5; // Согласно функциональным требованиям, автомобили с педальным приводом стоит продавать только покупателям с силой ног больше 5

    /// <summary>
    /// Переопределение метода ToString для педального двигателя
    /// </summary>
    public override string ToString() => $"Педальный двигатель. Размер педалей: {Size}";
}