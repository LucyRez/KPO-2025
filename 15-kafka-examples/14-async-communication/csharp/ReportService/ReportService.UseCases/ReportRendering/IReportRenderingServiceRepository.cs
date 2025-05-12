using ReportService.Entities;

namespace ReportService.UseCases.ReportRendering;

public interface IReportRenderingServiceRepository
{
    Task<IReadOnlyList<ReportedEvent>> GetReportedEventsAsync();
}

