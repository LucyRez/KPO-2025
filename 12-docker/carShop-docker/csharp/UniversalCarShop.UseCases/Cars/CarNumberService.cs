namespace UniversalCarShop.UseCases.Cars;

/// <summary>
/// Сервис для получения следующего номера автомобиля
/// </summary>
internal sealed class CarNumberService(ICarRepository carRepository) : ICarNumberService
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