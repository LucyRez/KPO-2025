using ReportServer.Client;
using UniversalCarShop.UseCases.Reports;

namespace UniversalCarShop.Infrastructure.Reports;

using ExternalReport = ReportServer.Client.Report;
using Report = UniversalCarShop.Entities.Common.Report;

internal sealed class ReportServerConnector : IReportServerConnector
{
    private readonly ReportServerClient _reportServerClient;

    public ReportServerConnector(ReportServerClient reportServerClient)
    {
        _reportServerClient = reportServerClient;
    }

    public void StoreReport(Report report)
    {
        var externalReport = new ExternalReport
        {
            Title = report.Title,
            Contents = report.Content,
        };

        _reportServerClient.StoreReportAsync(externalReport).Wait();
    }
}

