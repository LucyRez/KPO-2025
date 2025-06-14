using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewsAppBackend.Infrastructure.Database;
using NewsAppBackend.Infrastructure.UseCases.CreateDraft;
using NewsAppBackend.Infrastructure.UseCases.PublishDraft;
using NewsAppBackend.Infrastructure.UseCases.GetFeed;
using NewsAppBackend.Application.UseCases.CreateDraft;
using NewsAppBackend.Application.UseCases.PublishDraft;
using NewsAppBackend.Application.UseCases.GetFeed;


namespace NewsAppBackend.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<ReadWriteDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("ReadWrite"));
        });

        services.AddDbContext<ReadOnlyDbContext>((serviceProvider, options) =>
        {
            options
                .UseNpgsql(serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("ReadOnly"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddHostedService<MigrationRunner>();

        services.AddScoped<ICreateDraftRepository, CreateDraftRepository>();
        services.AddScoped<IPublishDraftRepository, PublishDraftRepository>();
        services.AddScoped<IGetFeedRepository, GetFeedRepository>();

        return services;
    }
}