
using System.Text.Json.Serialization;
using UniversalCarShop.Entities.Common;

namespace UniversalCarShop.Infrastructure.Database;

internal sealed class PedalEngineEntity(int pedalSize) : EngineEntityBase
{
    public int PedalSize { get; set; } = pedalSize;
    [JsonIgnore]
    public override IEngine DomainEngine => new PedalEngine(PedalSize);
}
