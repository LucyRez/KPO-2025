namespace ReportService.Infrastructure.ReportedEvents;

internal sealed record ReportedEventConsumerOptions(
    string Topic,
    string BootstrapServers
);

