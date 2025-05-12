using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniversalCarShop.Infrastructure.Database;
using UniversalCarShop.Infrastructure.Reports;
using UniversalCarShop.Infrastructure.Repositories;
using UniversalCarShop.UseCases.Cars;
using UniversalCarShop.UseCases.Customers;
using UniversalCarShop.UseCases.Reports;

namespace UniversalCarShop.Infrastructure;

public static class ServiceCollectionExtensions
{
    private const string ReportServerSectionPath = "ReportServer";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        services.AddReportServerConnector();
        
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("PostgreSQL");

            options.UseNpgsql(connectionString);
        });

        services.AddHostedService<DatabaseMigrator>();

        return services;
    }

    private static IServiceCollection AddReportServerConnector(this IServiceCollection services)
    {
        services.AddSingleton<IReportServerConnector>(sp =>
        {
            var options = sp.GetRequiredService<IConfiguration>()
                .GetSection(ReportServerSectionPath)
                .Get<ReportServerConnectorOptions>() ?? throw new InvalidOperationException("Report server options not found");

            var producer = new ProducerBuilder<string, string>(new ProducerConfig { BootstrapServers = options.BootstrapServers }).Build();

            return new ReportServerConnector(producer, options.Topic);
        });

        return services;
    }
}
