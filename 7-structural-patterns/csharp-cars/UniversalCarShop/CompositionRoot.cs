using Microsoft.Extensions.DependencyInjection;
using ReportServer.Client;
using UniversalCarShop.Cars;
using UniversalCarShop.Customers;
using UniversalCarShop.Reports;

namespace UniversalCarShop;

public static class CompositionRoot

{
    private static IServiceProvider? _services;

    public static IServiceProvider Services => _services ??= CreateServiceProvider();

    private static IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddSingleton<CarService>(); // регистрируем сервис для управления автомобилями
        services.AddSingleton<CustomersStorage>(); // регистрируем сервис для управления покупателями
        services.AddSingleton<PedalCarFactory>(); // регистрируем сервис для создания педальных автомобилей
        services.AddSingleton<HandCarFactory>(); // регистрируем сервис для создания автомобилей с ручным приводом


        services.AddSingleton<ICarProvider>(sp => sp.GetRequiredService<CarService>()); // регистрируем интерфейс ICarProvider в качестве сервиса
        services.AddSingleton<ICustomersProvider>(sp => sp.GetRequiredService<CustomersStorage>()); // регистрируем интерфейс ICustomersProvider в качестве сервиса

        services.AddSingleton<HseCarService>(); // регистрируем сервис по продаже автомобилей

        services.AddSingleton<ReportExporterFactory>(); // регистрируем фабрику экспортеров

        services.AddSingleton<CarShop>(); // регистрируем сервис для управления магазином автомобилей

        services.AddSingleton(sp => new ReportServerClient("http://localhost:5000")); // регистрируем сервис для управления сервером отчетов

        return services.BuildServiceProvider(); // строим контейнер зависимостей
    }
}