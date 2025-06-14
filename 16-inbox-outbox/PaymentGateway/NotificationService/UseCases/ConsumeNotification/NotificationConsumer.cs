using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationService.Database;
using PaymentService.Contracts.DTOs;

namespace NotificationService.UseCases.ConsumeNotification;

internal sealed class NotificationConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<NotificationConsumer> _logger;
    private readonly IConsumer<string, string> _consumer;
    private readonly string _topic;

    public NotificationConsumer(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<NotificationConsumer> logger,
        IConsumer<string, string> consumer,
        string topic)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _consumer = consumer;
        _topic = topic;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(_topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = _consumer.Consume(stoppingToken);

                if (result is null)
                {
                    continue;
                }

                var transactionChange = JsonSerializer.Deserialize<TransactionChangeDto>(result.Message.Value);

                if (transactionChange is null)
                {
                    _logger.LogWarning("Received invalid transaction change: {Message}", result.Message.Value);
                    continue;
                }

                await ProcessMessageAsync(transactionChange, stoppingToken);
                
                // Ручной коммит оффсета после успешной обработки сообщения
                _consumer.Commit(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while consuming messages");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }

    private async Task ProcessMessageAsync(TransactionChangeDto transactionChange, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<NotificationContext>();

        var notification = new Models.Notification(
            Guid.NewGuid(),
            GetNotificationKey(transactionChange),
            payload: JsonSerializer.Serialize(transactionChange),
            isProcessed: false,
            DateTimeOffset.UtcNow
        );

        await context.Notifications.AddAsync(notification, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Received notification {NotificationId}: {Payload}", notification.Id, notification.Payload);
    }

    private string GetNotificationKey(TransactionChangeDto transactionChange)
    {
        var notificationKey = transactionChange.TransactionId.ToString();
                
        notificationKey += transactionChange.Status switch
        {
            TransactionStatusType.Hold => "-hold",
            TransactionStatusType.Charge => "-charged",
            TransactionStatusType.Cancel => "-canceled",
            _ => throw new InvalidOperationException($"Unknown transaction status: {transactionChange.Status}")
        };

        return notificationKey;
    }
}