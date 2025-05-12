using ReportService.Entities;

namespace ReportService.UseCases.ReportedEvents;

public interface IReportedEventServiceRepository
{
    Task AddReportedEventAsync(ReportedEvent reportedEvent);
}
