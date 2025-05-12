using Microsoft.Extensions.DependencyInjection;
using ReportService.UseCases.ReportRendering;
using ReportService.UseCases.ReportRendering.Implementations;
using ReportService.UseCases.ReportedEvents;
using ReportService.UseCases.ReportedEvents.Implementations;

namespace ReportService.UseCases;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IReportRenderingService, ReportRenderingService>();
        services.AddScoped<IReportedEventService, ReportedEventService>();

        return services;
    }
}

