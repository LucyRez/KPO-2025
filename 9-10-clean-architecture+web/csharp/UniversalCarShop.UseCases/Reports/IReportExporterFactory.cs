namespace UniversalCarShop.UseCases.Reports;

public interface IReportExporterFactory
{
    ReportExporter Create(ReportFormat format);
}

