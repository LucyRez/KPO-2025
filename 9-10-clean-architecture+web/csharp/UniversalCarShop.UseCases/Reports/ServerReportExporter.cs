using UniversalCarShop.Entities.Common;
using UniversalCarShop.UseCases.Reports;

namespace UniversalCarShop.UseCases.Reports;

internal sealed class ServerReportExporter : ReportExporter
{
    private readonly IReportServerConnector _reportServerConnector;

    public ServerReportExporter(IReportServerConnector reportServerConnector)
    {
        _reportServerConnector = reportServerConnector;
    }

    public override void Export(Report report, TextWriter writer)
    {
        _reportServerConnector.StoreReport(report);
    }
}
