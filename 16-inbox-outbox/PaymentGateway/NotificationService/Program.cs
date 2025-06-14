using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationService.Database;
using NotificationService.UseCases.ConsumeNotification;
using NotificationService.UseCases.ProcessNotification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<MigrationRunner>();
        services.AddHostedService<NotificationProcessor>();
        services.AddHostedService(sp => {
            var consumer = sp.GetRequiredService<IConsumer<string, string>>();
            var topic = context.Configuration["Kafka:Topic"];
            var logger = sp.GetRequiredService<ILogger<NotificationConsumer>>();
            var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();

            return new NotificationConsumer(
                scopeFactory,
                logger,
                consumer,
                topic ?? throw new InvalidOperationException("Kafka topic is not configured")
            );
        });
        services.AddDbContext<NotificationContext>(options =>
            options.UseNpgsql(context.Configuration.GetConnectionString("Default")));
        
        services.AddSingleton<IConsumer<string, string>>(sp =>
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = context.Configuration["Kafka:BootstrapServers"],
                GroupId = context.Configuration["Kafka:GroupId"],
                EnableAutoCommit = false,
            };

            return new ConsumerBuilder<string, string>(config).Build();
        });
    })
    .Build();

await host.RunAsync();
