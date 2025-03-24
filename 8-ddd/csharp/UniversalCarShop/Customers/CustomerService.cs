using UniversalCarShop.Accounting;
using UniversalCarShop.Customers;

public sealed class CustomerService(PendingCommandService pendingCommandService, ICustomerRepository customerRepository)
{
    public void AddCustomerPending(string name, int legPower, int handPower)
    {
        var command = new AddCustomerCommand(customerRepository, name, legPower, handPower);

        pendingCommandService.AddCommand(command);
    }
}