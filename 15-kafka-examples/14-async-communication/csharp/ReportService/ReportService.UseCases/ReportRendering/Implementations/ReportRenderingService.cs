using System.Text;
using ReportService.Entities;

namespace ReportService.UseCases.ReportRendering.Implementations;

internal sealed class ReportRenderingService : IReportRenderingService
{
    private readonly IReportRenderingServiceRepository _reportRenderingServiceRepository;

    public ReportRenderingService(IReportRenderingServiceRepository reportRenderingServiceRepository)
    {
        _reportRenderingServiceRepository = reportRenderingServiceRepository;
    }

    public async Task<string> RenderAsHtmlAsync()
    {
        var reportedEvents = await _reportRenderingServiceRepository.GetReportedEventsAsync();

        var html = new StringBuilder();

        html.AppendLine("<html>");
        html.AppendLine("<body>");
        html.AppendLine("<h1>Report</h1>");
        WriteReportedEvents(html, reportedEvents);
        html.AppendLine("</body>");
        html.AppendLine("</html>");

        return html.ToString();
    }

    private static void WriteReportedEvents(StringBuilder html, IReadOnlyList<ReportedEvent> reportedEvents)
    {
        html.AppendLine("<table>");
        html.AppendLine("<tr>");
        html.AppendLine("<th>Event Type</th>");
        html.AppendLine("<th>Event Description</th>");
        html.AppendLine("<th>Occured On</th>");
        html.AppendLine("</tr>");

        foreach (var reportedEvent in reportedEvents)
        {
            html.AppendLine($"<tr>");
            html.AppendLine($"<td>{reportedEvent.EventType}</td>");
            html.AppendLine($"<td>{reportedEvent.EventDescription}</td>");
            html.AppendLine($"<td>{reportedEvent.OccuredOn}</td>");
            html.AppendLine($"</tr>");
        }

        html.AppendLine("</table>");
    }
}
