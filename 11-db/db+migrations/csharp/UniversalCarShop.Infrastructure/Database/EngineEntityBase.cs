using System.Text.Json.Serialization;
using UniversalCarShop.Entities.Common;

namespace UniversalCarShop.Infrastructure.Database;

[JsonDerivedType(typeof(PedalEngineEntity), nameof(PedalEngineEntity))]
[JsonDerivedType(typeof(HandEngineEntity), nameof(HandEngineEntity))]
internal abstract class EngineEntityBase
{
    [JsonIgnore]
    public abstract IEngine DomainEngine { get; }
}
