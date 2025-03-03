using System;
using UniversalCarShop.Customers;

namespace UniversalCarShop.Engines;

public class HandEngine : IEngine
{
    /// <summary>
    /// Реализация метода опредения совместимости
    /// </summary>
    public bool IsCompatible(Customer customer) => customer.HandPower > 5; // Согласно функциональным требованиям, автомобили с ручным приводом следует продавать только покупателям с силой рук больше 5

    /// <summary>
    /// Переопределение метода ToString для двигателя с ручным приводом
    /// </summary>
    public override string ToString() => "Двигатель с ручным приводом";
}
