namespace UniversalCarShop.Cars;

/// <summary>
/// Сервис для получения следующего номера автомобиля
/// </summary>
public sealed class CarNumberService(ICarRepository carRepository)
{
    /// <summary>
    /// Получает следующий номер автомобиля
    /// </summary>
    public int GetNextNumber() => carRepository
        .GetAll()
        .Select(c => c.Number)
        .DefaultIfEmpty(0)
        .Max() + 1;
}