using NewsAppBackend.Application.Common.Abstractions;
using NewsAppBackend.Application.UseCases.GetFeed;

namespace NewsAppBackend.WebApi.Endpoints;

public static class GetFeed
{
    public static void MapGetFeed(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/feed", async (
            int page,
            int pageSize,
            IQueryHandler<GetFeedQuery, FeedDto> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetFeedQuery(page, pageSize);
            var result = await handler.HandleAsync(query, cancellationToken);
            return Results.Ok(result);
        })
        .WithName("GetFeed")
        .WithOpenApi();
    }
}