namespace ReportService.UseCases.ReportedEvents.DTOs;

public sealed record ReportedEventInDto(
    string EventType,
    string EventDescription,
    DateTimeOffset OccuredOn);
