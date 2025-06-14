using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationService.Database;

namespace NotificationService.UseCases.ProcessNotification;

internal sealed class NotificationProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<NotificationProcessor> _logger;

    public NotificationProcessor(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<NotificationProcessor> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = await ProcessNotificationAsync(stoppingToken);

                if (result == ProcessResult.AllProcessed)
                {
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                    continue;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing notifications");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }

    private async Task<ProcessResult> ProcessNotificationAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<NotificationContext>();

        var notifications = await context.Notifications
            .Where(n => !n.IsProcessed)
            .OrderBy(n => n.CreatedAt)
            .Take(2)
            .ToListAsync(cancellationToken);

        if (!notifications.Any())
        {
            return ProcessResult.AllProcessed;
        }

        var notification = notifications.First();

        _logger.LogInformation("Processing notification {NotificationId}: {Payload}", notification.Id, notification.Payload);

        await context.Notifications
            .Where(n => n.Id == notification.Id)
            .ExecuteUpdateAsync(
                n => n.SetProperty(n => n.IsProcessed, true),
                cancellationToken
            );

        return notifications.Count == 1
            ? ProcessResult.AllProcessed
            : ProcessResult.HasMore;
    }

    private enum ProcessResult
    {
        AllProcessed,
        HasMore
    }
}