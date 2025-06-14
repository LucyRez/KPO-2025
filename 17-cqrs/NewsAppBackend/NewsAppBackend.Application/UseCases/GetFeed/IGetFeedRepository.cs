using NewsAppBackend.Domain.Entities;

namespace NewsAppBackend.Application.UseCases.GetFeed;

public interface IGetFeedRepository
{
    Task<IEnumerable<FeedItem>> GetFeedItemsAsync(int page, int pageSize, CancellationToken cancellationToken);
}