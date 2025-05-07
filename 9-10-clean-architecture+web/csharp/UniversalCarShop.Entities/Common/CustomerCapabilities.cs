namespace UniversalCarShop.Entities.Common;

public sealed record CustomerCapabilities
{
    public int LegPower { get; }
    public int HandPower { get; }

    public CustomerCapabilities(int legPower, int handPower)
    {
        if (legPower < 0)
            throw new ArgumentException("Сила ног не может быть отрицательной", nameof(legPower));
        
        if (handPower < 0)
            throw new ArgumentException("Сила рук не может быть отрицательной", nameof(handPower));
        
        LegPower = legPower;
        HandPower = handPower;
    }
}