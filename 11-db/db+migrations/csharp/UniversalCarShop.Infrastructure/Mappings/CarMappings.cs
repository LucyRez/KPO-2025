using UniversalCarShop.Entities.Common;
using UniversalCarShop.Infrastructure.Database;

namespace UniversalCarShop.Infrastructure.Mappings;

internal static class CarMappings
{
    public static Car ToDomain(this CarEntity carEntity) => new(
        number: carEntity.Number,
        engine: carEntity.Engine.DomainEngine
    );

    public static CarEntity ToEntity(this Car car) => new(
        Number: car.Number,
        Engine: car.Engine.ToEntity()
    );
}

