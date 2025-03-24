using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReportServer.Client;
using UniversalCarShop.Infrastructure.Reports;
using UniversalCarShop.Infrastructure.Repositories;
using UniversalCarShop.UseCases.Cars;
using UniversalCarShop.UseCases.Customers;
using UniversalCarShop.UseCases.Reports;

namespace UniversalCarShop.Infrastructure;

public static class ServiceCollectionExtensions
{
    private const string ReportServerUrlPath = "ReportServer:Url";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IReportServerConnector, ReportServerConnector>();
        services.AddSingleton<ICarRepository, CarRepository>();
        services.AddSingleton<ICustomerRepository, CustomerRepository>();

        services.AddSingleton(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var reportServerUrl = configuration.GetSection(ReportServerUrlPath).Value
                ?? throw new InvalidOperationException($"Report server URL not found in configuration at path: {ReportServerUrlPath}");

            return new ReportServerClient(reportServerUrl);
        });
        
        return services;
    }
}
