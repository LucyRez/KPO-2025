using System;
using UniversalCarShop.Customers;

namespace UniversalCarShop.Cars;

/// <summary>
/// Класс для управления автомобилями
/// </summary>
public class CarService : ICarProvider
{
    /// <summary>
    /// Коллекция для хранения автомобилей
    /// </summary>
    private readonly LinkedList<Car> _cars = new(); // Используем связный список, так как он позволяет оптимально удалять элементы из середины

    /// <summary>
    /// Поле-счетчик для определения номеров автомобилей
    /// </summary>
    private int _carNumberCounter;

    /// <summary>
    /// Метод для получения подходящего автомобиля
    /// </summary>
    public Car? TakeCar(Customer customer)
    {
        // Пробегаемся по коллекции с использованием цикла for.
        // Мы не можем использовать foreach, так как нам необходимо будет удалить автомобиль из списка, когда мы его найдем.
        for (var node = _cars.First; node != null; node = node.Next)
        {
            if (!node.Value.IsCompatible(customer))
            {
                continue; // если текущий автомобиль несовместим с покупателем - пропускаем
            }

            // если мы здесь, то автомобиль совместим с покупателем.
            // значит удалим его из списка и вернем в качестве результата.
            _cars.Remove(node);

            return node.Value;
        }

        // если мы здесь, то это значит, что мы прошли весь цикл и не нашли ни одного подходящего автомобиля.
        // значит вернем null

        return null;
    }

    /// <summary>
    /// Методя для добавления автомобиля
    /// </summary>
    public void AddCar<TParams>(ICarFactory<TParams> carFactory, TParams carParams)
    {
        // создаем автомобиль из переданной фабрики
        var car = carFactory.CreateCar(
            carParams, // передаем параметры
            ++_carNumberCounter // передаем номер - номер будет начинаться с 1
        );

        _cars.AddLast(car); // добавляем автомобиль в конец списка
    }
}
