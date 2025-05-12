using ReportService.Entities;
using ReportService.UseCases.ReportedEvents.DTOs;

namespace ReportService.UseCases.ReportedEvents.Implementations;

internal sealed class ReportedEventService : IReportedEventService
{
    private readonly IReportedEventServiceRepository _reportedEventServiceRepository;

    public ReportedEventService(IReportedEventServiceRepository reportedEventServiceRepository)
    {
        _reportedEventServiceRepository = reportedEventServiceRepository;
    }

    public async Task AddReportedEventAsync(ReportedEventInDto reportedEvent)
    {
        var reportedEventEntity = new ReportedEvent(
            Guid.NewGuid(),
            reportedEvent.EventType,
            reportedEvent.EventDescription,
            reportedEvent.OccuredOn
        );
        
        await _reportedEventServiceRepository.AddReportedEventAsync(reportedEventEntity);
    }
}
