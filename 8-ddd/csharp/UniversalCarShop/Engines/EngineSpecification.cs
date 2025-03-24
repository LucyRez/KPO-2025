using UniversalCarShop.Customers;

namespace UniversalCarShop.Engines;

public sealed record EngineSpecification
{
    public EngineSpecification(int requiredLegPower, int requiredHandPower, string type)
    {
        if (requiredLegPower < 0)
            throw new ArgumentException("Требуемая сила ног не может быть отрицательной", nameof(requiredLegPower));
        
        if (requiredHandPower < 0)
            throw new ArgumentException("Требуемая сила рук не может быть отрицательной", nameof(requiredHandPower));
        
        Type = type;
        RequiredLegPower = requiredLegPower;
        RequiredHandPower = requiredHandPower;
    }

    public int RequiredLegPower { get; }
    public int RequiredHandPower { get; }
    public string Type { get; }

    public bool IsCompatibleWith(CustomerCapabilities capabilities)
    {
        if (capabilities == null)
            throw new ArgumentNullException(nameof(capabilities));
        
        return capabilities.LegPower >= RequiredLegPower && 
                capabilities.HandPower >= RequiredHandPower;
    }
}