using ReportService.UseCases.ReportRendering;
using System.Text;
namespace ReportService.Web.Endpoints;

public static class ReportRenderingEndpoints
{
    public static WebApplication MapReportRenderingEndpoints(this WebApplication app)
    {
        app.MapGet("/api/v1/report/html", async (IReportRenderingService reportRenderingService) =>
        {
            var report = await reportRenderingService.RenderAsHtmlAsync();

            return Results.Text(report, "text/html", Encoding.UTF8);
        })
        .WithName("GetReportHtml")
        .WithOpenApi();

        return app;
    }
}