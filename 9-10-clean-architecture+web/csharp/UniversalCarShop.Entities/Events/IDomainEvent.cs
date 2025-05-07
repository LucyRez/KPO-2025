namespace UniversalCarShop.Entities.Events;

/// <summary>
/// Интерфейс для событий предметной области
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Дата и время события
    /// </summary>
    DateTime OccurredOn { get; }
}