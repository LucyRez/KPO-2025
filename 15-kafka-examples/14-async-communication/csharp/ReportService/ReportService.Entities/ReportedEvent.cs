namespace ReportService.Entities;

public sealed record ReportedEvent(
    Guid eventId,
    string eventType,
    string eventDescription,
    DateTimeOffset occuredOn
)
{
    /// <summary>
    /// Идентификатор события
    /// </summary>
    public Guid EventId { get; } = eventId;

    /// <summary>
    /// Тип события
    /// </summary>
    public string EventType { get; } = eventType;

    /// <summary>
    /// Описание события
    /// </summary>
    public string EventDescription { get; } = eventDescription;

    /// <summary>
    /// Время возникновения события
    /// </summary>
    public DateTimeOffset OccuredOn { get; } = occuredOn;
}

