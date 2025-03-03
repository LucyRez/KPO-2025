using System;

namespace UniversalCarShop.Customers;

/// <summary>
/// Хранилище покупателей
/// </summary>
public class CustomersStorage : ICustomersProvider // хранилище реализует интерфейс для предоставления списка покупателей
{
    /// <summary>
    /// Список покупателей
    /// </summary>
    private readonly List<Customer> _customers = new(); // используем обычный список, так как нам не потребуется делать каких-то специфических операций

    /// <summary>
    /// Реализация метода для получения списка покупателей 
    /// </summary>
    public IReadOnlyCollection<Customer> GetCustomers() => _customers; // возвращаем список покупателей

    /// <summary>
    /// Метод для добавления покупателя
    /// </summary>
    public void AddCustomer(Customer customer)
    {
        _customers.Add(customer); // просто добавляем покупателя в список
    }
}
