namespace ReportService.UseCases.ReportRendering;

public interface IReportRenderingService
{
    Task<string> RenderAsHtmlAsync();
}

