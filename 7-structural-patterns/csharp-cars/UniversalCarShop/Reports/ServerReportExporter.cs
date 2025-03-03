using UniversalCarShop.Reports;
using ReportServer.Client;

using ExternalReport = ReportServer.Client.Report;

namespace UniversalCarShop.Reports;

public sealed class ServerReportExporter : ReportExporter
{
    private readonly ReportServerClient _reportServerClient;

    public ServerReportExporter(ReportServerClient reportServerClient)
    {
        _reportServerClient = reportServerClient;
    }

    public override void Export(Report report, TextWriter writer)
    {
        var externalReport = new ExternalReport
        {
            Title = report.Title,
            Contents = report.Content
        };

        _reportServerClient.StoreReportAsync(externalReport).Wait();
    }
}
