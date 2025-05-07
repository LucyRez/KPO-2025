namespace UniversalCarShop.UseCases.Customers;

public interface ICustomerService
{
    void AddCustomerPending(string name, int legPower, int handPower);
}

