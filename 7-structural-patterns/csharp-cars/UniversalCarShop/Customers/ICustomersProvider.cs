using System;

namespace UniversalCarShop.Customers;

/// <summary>
/// Интерфейс, предоставляющий список покупателей
/// </summary>
public interface ICustomersProvider
{
    /// <summary>
    /// Метод для получения списка покупателей
    /// </summary>
    IReadOnlyCollection<Customer> GetCustomers(); // метод возвращает коллекцию только для чтения, так как мы не хотим давать вызывающему коду возможность изменять список
}
