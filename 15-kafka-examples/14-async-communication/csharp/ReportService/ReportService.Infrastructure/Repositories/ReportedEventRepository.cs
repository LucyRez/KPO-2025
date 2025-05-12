using ReportService.Entities;
using ReportService.UseCases.ReportedEvents;
using ReportService.UseCases.ReportRendering;

namespace ReportService.Infrastructure.Repositories;

internal sealed class ReportedEventRepository : IReportedEventServiceRepository, IReportRenderingServiceRepository
{
    private readonly List<ReportedEvent> _reportedEvents = new();

    public Task AddReportedEventAsync(ReportedEvent reportedEvent)
    {
        _reportedEvents.Add(reportedEvent);

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<ReportedEvent>> GetReportedEventsAsync()
    {
        return Task.FromResult(_reportedEvents as IReadOnlyList<ReportedEvent>);
    }
}

