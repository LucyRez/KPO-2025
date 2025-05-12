using ReportService.UseCases.ReportedEvents.DTOs;

namespace ReportService.UseCases.ReportedEvents;

public interface IReportedEventService
{
    Task AddReportedEventAsync(ReportedEventInDto reportedEvent);
}

