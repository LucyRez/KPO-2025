using Microsoft.AspNetCore.Mvc;
using ReportService.UseCases.ReportedEvents;
using ReportService.UseCases.ReportedEvents.DTOs;

namespace ReportService.Web.Endpoints;

public static class ReportEventEndpoints
{
    public static WebApplication MapReportEventEndpoints(this WebApplication app)
    {
        app.MapPost("/api/v1/report-event", async ([FromBody] ReportedEventInDto dto, IReportedEventService reportedEventService) =>
        {
            await reportedEventService.AddReportedEventAsync(dto);

            return Results.Ok();
        })
        .WithName("AddReportEvent")
        .WithOpenApi();

        return app;
    }
}