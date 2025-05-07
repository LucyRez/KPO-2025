using Microsoft.Extensions.Hosting;

namespace UniversalCarShop.UseCases.Reports;

internal sealed class ReportingServiceInitializer : IHostedService
{
    private readonly ReportingService _reportingService;

    public ReportingServiceInitializer(ReportingService reportingService)
    {
        _reportingService = reportingService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _reportingService.Initialize();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}