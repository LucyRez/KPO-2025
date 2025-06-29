using Microsoft.EntityFrameworkCore;

namespace PaymentService.Database;

internal sealed class MigrationRunner : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public MigrationRunner(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        context.Database.Migrate();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}