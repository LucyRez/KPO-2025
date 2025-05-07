using Microsoft.Extensions.DependencyInjection;
using ReportServer.Client;
using UniversalCarShop.Accounting;
using UniversalCarShop.Cars;
using UniversalCarShop.Customers;
using UniversalCarShop.Reports;
using UniversalCarShop.Sales;
using UniversalCarShop.Domain;
namespace UniversalCarShop;

public static class CompositionRoot
{
    private static IServiceProvider? _services;

    public static IServiceProvider Services => _services ??= CreateServiceProvider();

    private static IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddSingleton<ICarRepository, CarRepository>(); // регистрируем репозиторий для автомобилей
        services.AddSingleton<ICustomerRepository, CustomerRepository>(); // регистрируем репозиторий для покупателей
        services.AddSingleton<PedalCarFactory>(); // регистрируем сервис для создания педальных автомобилей
        services.AddSingleton<HandCarFactory>(); // регистрируем сервис для создания автомобилей с ручным приводом
        services.AddSingleton<ReportExporterFactory>(); // регистрируем фабрику экспортеров
        services.AddSingleton<ReportBuilder>(); // регистрируем билдер отчетов
        services.AddSingleton<CarInventoryService>(); // регистрируем сервис для работы с автомобилями
        services.AddSingleton<CustomerService>(); // регистрируем сервис для работы с покупателями
        services.AddSingleton<SalesService>(); // регистрируем сервис для продажи автомобилей
        services.AddSingleton<ReportingService>(); // регистрируем сервис для отчетности
        services.AddSingleton<PendingCommandService>(); // регистрируем сервис для отложенных команд
        services.AddSingleton<CarNumberService>(); // регистрируем сервис для работы с номерами автомобилей
        services.AddSingleton<IDomainEventService, DomainEventService>(); // регистрируем сервис для работы с событиями предметной области

        services.AddSingleton(sp => new ReportServerClient("http://localhost:5000")); // регистрируем сервис для управления сервером отчетов

        return services.BuildServiceProvider();
    }

    public static CustomerService CustomerService { get; } = Services.GetRequiredService<CustomerService>();
    public static CarInventoryService CarInventoryService { get; } = Services.GetRequiredService<CarInventoryService>();
    public static SalesService SalesService { get; } = Services.GetRequiredService<SalesService>();
    public static ReportingService ReportingService { get; } = Services.GetRequiredService<ReportingService>();
    public static PendingCommandService PendingCommandService { get; } = Services.GetRequiredService<PendingCommandService>();
}