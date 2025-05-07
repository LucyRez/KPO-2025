namespace UniversalCarShop.Entities.Common;

public sealed class HandEngine : IEngine
{
    public HandEngine()
    {
        // Требуемая сила рук должна быть выше 5
        Specification = new EngineSpecification(
            requiredLegPower: 0, // Для ручного двигателя не требуется сила ног
            requiredHandPower: 5, // Минимальная требуемая сила рук
            type: "С ручным приводом"
        );
    }

    public EngineSpecification Specification { get; }

    public bool IsCompatible(CustomerCapabilities capabilities)
    {
        if (capabilities == null)
            throw new ArgumentNullException(nameof(capabilities));
        
        return Specification.IsCompatibleWith(capabilities);
    }

    public override string ToString() => $"Ручной двигатель (требуемая сила рук: {Specification.RequiredHandPower})";
}
