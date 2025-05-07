using UniversalCarShop.Entities.Common;
using UniversalCarShop.Infrastructure.Database;

namespace UniversalCarShop.Infrastructure.Mappings;

internal static class CustomerMappings
{
    public static Customer ToDomain(this CustomerEntity customerEntity) => new(
        name: customerEntity.Name,
        capabilities: new CustomerCapabilities(
            legPower: customerEntity.LegPower,
            handPower: customerEntity.HandPower
        ),
        car: customerEntity.Car?.ToDomain()
    );

    public static CustomerEntity ToEntity(this Customer customer) => new(
        Name: customer.Name,
        LegPower: customer.Capabilities.LegPower,
        HandPower: customer.Capabilities.HandPower,
        CarNumber: customer.Car?.Number
    );
}

