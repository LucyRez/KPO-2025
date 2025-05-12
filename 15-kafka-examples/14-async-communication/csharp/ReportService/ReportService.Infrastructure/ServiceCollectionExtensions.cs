using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReportService.Infrastructure.ReportedEvents;
using ReportService.Infrastructure.Repositories;
using ReportService.UseCases.ReportedEvents;
using ReportService.UseCases.ReportRendering;

namespace ReportService.Infrastructure;

public static class ServiceCollectionExtensions
{
    private const string ReportedEventsConsumerSectionPath = "ReportedEventsConsumer";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ReportedEventRepository>();
        services.AddSingleton<IReportedEventServiceRepository>(sp => sp.GetRequiredService<ReportedEventRepository>());
        services.AddSingleton<IReportRenderingServiceRepository>(sp => sp.GetRequiredService<ReportedEventRepository>());
        services.AddReportedEventConsumer();

        return services;
    }

    private static IServiceCollection AddReportedEventConsumer(this IServiceCollection services)
    {
        services.AddHostedService(sp =>
        {
            var options = sp.GetRequiredService<IConfiguration>()
                .GetSection(ReportedEventsConsumerSectionPath)
                .Get<ReportedEventConsumerOptions>() ?? throw new InvalidOperationException("Reported events consumer options not found");

            var consumer = new ConsumerBuilder<string, string>(new ConsumerConfig {
                BootstrapServers = options.BootstrapServers,
                GroupId = "report-service-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            }).Build();

            var serviceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();

            return new ReportedEventConsumer(consumer, options.Topic, serviceScopeFactory);
        });

        return services;
    }
}
