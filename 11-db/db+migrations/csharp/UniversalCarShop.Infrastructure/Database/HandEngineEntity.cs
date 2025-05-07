using System.Text.Json.Serialization;
using UniversalCarShop.Entities.Common;

namespace UniversalCarShop.Infrastructure.Database;

internal sealed class HandEngineEntity : EngineEntityBase
{
    [JsonIgnore]
    public override IEngine DomainEngine => new HandEngine();
}

