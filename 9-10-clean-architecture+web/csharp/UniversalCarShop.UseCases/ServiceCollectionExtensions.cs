using Microsoft.Extensions.DependencyInjection;
using UniversalCarShop.UseCases.Cars;
using UniversalCarShop.UseCases.Customers;
using UniversalCarShop.UseCases.Events;
using UniversalCarShop.UseCases.Sales;
using UniversalCarShop.UseCases.PendingCommands;
using UniversalCarShop.UseCases.Reports;
using UniversalCarShop.UseCases.Engines;


namespace UniversalCarShop.UseCases;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        // Сервисы для работы с автомобилями
        services.AddSingleton<ICarInventoryService, CarInventoryService>();
        services.AddSingleton<ICarNumberService, CarNumberService>();
        services.AddSingleton<ICarFactory<HandEngineParams>, HandCarFactory>();
        services.AddSingleton<ICarFactory<PedalEngineParams>, PedalCarFactory>();

        // Сервисы для работы с покупателями
        services.AddSingleton<ICustomerService, CustomerService>();

        // Сервисы для работы с событиями
        services.AddSingleton<IDomainEventService, DomainEventService>();

        // Сервисы для работы с продажами
        services.AddSingleton<ISalesService, SalesService>();

        // Сервисы для работы с отчетами
        services.AddSingleton<ReportingService>();
        services.AddSingleton<IReportingService>(sp => sp.GetRequiredService<ReportingService>());
        services.AddSingleton<IReportExporterFactory, ReportExporterFactory>();
        services.AddSingleton<ReportBuilder>();

        // Сервисы для работы с отложенными командами
        services.AddSingleton<IPendingCommandService, PendingCommandService>();

        // Инициализация сервисов
        services.AddHostedService<ReportingServiceInitializer>();
        return services;
    }
}
