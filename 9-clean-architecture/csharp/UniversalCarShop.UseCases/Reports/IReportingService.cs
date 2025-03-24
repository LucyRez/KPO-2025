using UniversalCarShop.Entities.Common;

namespace UniversalCarShop.UseCases.Reports;

public interface IReportingService
{
    Report GetCurrentReport();
    void ExportReport(ReportFormat format, TextWriter writer);
}

