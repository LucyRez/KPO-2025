using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReportService.UseCases.ReportedEvents;
using ReportService.UseCases.ReportedEvents.DTOs;

namespace ReportService.Infrastructure.ReportedEvents;

internal sealed class ReportedEventConsumer : BackgroundService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly string _topic;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ReportedEventConsumer(
        IConsumer<string, string> consumer,
        string topic,
        IServiceScopeFactory serviceScopeFactory)
    {
        _consumer = consumer;
        _topic = topic;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        _consumer.Subscribe(_topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            var result = _consumer.Consume(stoppingToken);

            if (result.IsPartitionEOF)
            {
                continue;
            }

            var reportedEvent = JsonSerializer.Deserialize<ReportedEventInDto>(result.Message.Value);

            if (reportedEvent is null)
            {
                continue;
            }

            await HandleReportedEventAsync(reportedEvent);
        }

        _consumer.Close();
    }

    private async Task HandleReportedEventAsync(ReportedEventInDto reportedEvent)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var reportedEventService = scope.ServiceProvider.GetRequiredService<IReportedEventService>();
        await reportedEventService.AddReportedEventAsync(reportedEvent);
    }
}