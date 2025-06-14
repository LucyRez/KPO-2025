using NewsAppBackend.Application.Common.Abstractions;

namespace NewsAppBackend.Application.UseCases.GetFeed;

internal sealed class GetFeedQueryHandler(
    IGetFeedRepository repository
) : IQueryHandler<GetFeedQuery, FeedDto>
{
    public async Task<FeedDto> HandleAsync(GetFeedQuery query, CancellationToken cancellationToken)
    {
        var feedItems = await repository.GetFeedItemsAsync(query.Page, query.PageSize, cancellationToken);

        var items = feedItems.Select(item => new FeedItemDto(
            item.Id,
            item.Title,
            item.Content,
            item.CreatedAt
        )).ToList();

        return new FeedDto(items);
    }
}