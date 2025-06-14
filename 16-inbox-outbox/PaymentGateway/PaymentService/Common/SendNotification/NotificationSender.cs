using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using PaymentService.Database;

namespace PaymentService.Common.SendNotification;

internal sealed class NotificationSender : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<NotificationSender> _logger;
    private readonly IProducer<string, string> _producer;
    private readonly string _topic;

    public NotificationSender(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<NotificationSender> logger,
        IProducer<string, string> producer,
        string topic)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _producer = producer;
        _topic = topic;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = await SendNotificationAsync(stoppingToken);

                if (result == SendResult.AllSent)
                {
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                    continue;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending notifications");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }

    private async Task<SendResult> SendNotificationAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PaymentContext>();

        var notifications = await context.TransactionChangeNotifications
            .Where(n => !n.IsSent)
            .OrderBy(n => n.CreatedAt)
            .Take(2)
            .ToListAsync(cancellationToken);

        if (!notifications.Any())
        {
            return SendResult.AllSent;
        }

        var notification = notifications.First();

        var result = await _producer.ProduceAsync(_topic, new Message<string, string>
        {
            Key = notification.Id.ToString(),
            Value = notification.Payload
        }, cancellationToken);

        if (result.Status == PersistenceStatus.Persisted)
        {
            await context.TransactionChangeNotifications
                .Where(n => n.Id == notification.Id)
                .ExecuteUpdateAsync(
                    n => n.SetProperty(n => n.IsSent, true),
                    cancellationToken
                );
        }

        return notifications.Count == 1
            ? SendResult.AllSent
            : SendResult.HasMore;
    }

    private enum SendResult
    {
        AllSent,
        HasMore
    }
}