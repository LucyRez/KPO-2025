namespace UniversalCarShop.UseCases.Reports;

public sealed record ReportedEventDto(
    string EventType,
    string EventDescription,
    DateTimeOffset OccuredOn
);