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
        services.AddScoped<ICarInventoryService, CarInventoryService>();
        services.AddScoped<ICarNumberService, CarNumberService>();
        services.AddSingleton<ICarFactory<HandEngineParams>, HandCarFactory>();
        services.AddSingleton<ICarFactory<PedalEngineParams>, PedalCarFactory>();

        // Сервисы для работы с покупателями
        services.AddScoped<ICustomerService, CustomerService>();

        // Сервисы для работы с событиями
        services.AddSingleton<IDomainEventService, DomainEventService>();

        // Сервисы для работы с продажами
        services.AddScoped<ISalesService, SalesService>();

        // Сервисы для работы с отчетами
        services.AddSingleton<ReportingService>();

        // Сервисы для работы с отложенными командами
        services.AddSingleton<IPendingCommandService, PendingCommandService>();

        // Инициализация сервисов
        services.AddHostedService<ReportingServiceInitializer>();

        // Сервисы для работы с базой данных
        return services;
    }
}
