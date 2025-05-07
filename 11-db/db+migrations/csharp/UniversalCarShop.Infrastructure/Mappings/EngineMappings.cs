using UniversalCarShop.Entities.Common;
using UniversalCarShop.Infrastructure.Database;

namespace UniversalCarShop.Infrastructure.Mappings;

internal static class EngineMappings
{
    public static EngineEntityBase ToEntity(this IEngine engine) => engine switch
    {
        PedalEngine pedalEngine => new PedalEngineEntity(pedalEngine.PedalSize),
        HandEngine handEngine => new HandEngineEntity(),
        _ => throw new ArgumentException("Unsupported engine type", nameof(engine))
    };
}
