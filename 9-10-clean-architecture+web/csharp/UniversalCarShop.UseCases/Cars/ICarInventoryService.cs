namespace UniversalCarShop.UseCases.Cars;

public interface ICarInventoryService
{
    void AddPedalCarPending(int pedalSize);
    void AddHandCarPending();
}

