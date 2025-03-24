namespace UniversalCarShop.Entities.Common;

/// <summary>
/// Педальный двигатель
/// </summary>
public sealed class PedalEngine : IEngine
{
    public PedalEngine(int pedalSize)
    {
        if (pedalSize <= 0)
            throw new ArgumentException("Размер педалей должен быть положительным числом", nameof(pedalSize));
        
        // Создаем спецификацию двигателя на основе размера педалей
        // Требуемая сила ног должна быть выше 5
        Specification = new EngineSpecification(
            requiredLegPower: 5, // Минимальная требуемая сила ног
            requiredHandPower: 0, // Для педального двигателя не требуется сила рук
            type: "Педальный"
        );
    }

    public EngineSpecification Specification { get; }

    public bool IsCompatible(CustomerCapabilities capabilities)
    {
        if (capabilities == null)
            throw new ArgumentNullException(nameof(capabilities));
        
        return Specification.IsCompatibleWith(capabilities);
    }

    public override string ToString() => $"Педальный двигатель (требуемая сила ног: {Specification.RequiredLegPower})";
}