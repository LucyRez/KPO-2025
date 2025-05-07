using UniversalCarShop.Entities.Common;

namespace UniversalCarShop.UseCases.Cars;

/// <summary>
/// Интерфейс для репозитория автомобилей
/// </summary>
public interface ICarRepository
{
    /// <summary>
    /// Добавление автомобиля в репозиторий
    /// </summary>
    void Add(Car car);

    /// <summary>
    /// Поиск совместимого автомобиля для покупателя
    /// </summary>
    Car? FindCompatibleCar(CustomerCapabilities capabilities);

    /// <summary>
    /// Получение всех автомобилей
    /// </summary>
    IEnumerable<Car> GetAll();
}