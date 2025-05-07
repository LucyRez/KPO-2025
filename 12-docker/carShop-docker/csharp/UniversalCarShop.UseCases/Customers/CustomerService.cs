using Microsoft.Extensions.DependencyInjection;
using UniversalCarShop.UseCases.DTOs;
using UniversalCarShop.UseCases.PendingCommands;

namespace UniversalCarShop.UseCases.Customers;

internal sealed class CustomerService(
    IServiceScopeFactory serviceScopeFactory,
    IPendingCommandService pendingCommandService,
    ICustomerRepository customerRepository) : ICustomerService
{
    public void AddCustomerPending(string name, int legPower, int handPower)
    {
        var command = new AddCustomerCommand(serviceScopeFactory, name, legPower, handPower);

        pendingCommandService.AddCommand(command);
    }

    public IEnumerable<CustomerDto> GetAllCustomers() => customerRepository
        .GetAll()
        .Select(c => new CustomerDto(
            c.Name,
            c.Capabilities.LegPower,
            c.Capabilities.HandPower,
            c.Car is not null ? new CarDto(
                c.Car.Number,
                c.Car.Engine.Specification.Type) : null));
}