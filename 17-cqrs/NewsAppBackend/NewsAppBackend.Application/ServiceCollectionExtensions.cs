using Microsoft.Extensions.DependencyInjection;
using NewsAppBackend.Application.Common.Abstractions;
using NewsAppBackend.Application.UseCases.CreateDraft;
using NewsAppBackend.Application.UseCases.GetFeed;
using NewsAppBackend.Application.UseCases.PublishDraft;

namespace NewsAppBackend.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateDraftCommand, CreatedDraftDto>, CreateDraftCommandHandler>();
        services.AddScoped<ICommandHandler<PublishDraftCommand, PublishedDraftDto>, PublishDraftCommandHandler>();
        services.AddScoped<IQueryHandler<GetFeedQuery, FeedDto>, GetFeedQueryHandler>();
        
        return services;
    }
}