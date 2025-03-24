using UniversalCarShop.UseCases.PendingCommands;

namespace UniversalCarShop.UseCases.Customers;

internal sealed class CustomerService(IPendingCommandService pendingCommandService, ICustomerRepository customerRepository) : ICustomerService
{
    public void AddCustomerPending(string name, int legPower, int handPower)
    {
        var command = new AddCustomerCommand(customerRepository, name, legPower, handPower);

        pendingCommandService.AddCommand(command);
    }
}