using Microsoft.EntityFrameworkCore;
using NewsAppBackend.Application.UseCases.GetFeed;
using NewsAppBackend.Domain.Entities;
using NewsAppBackend.Infrastructure.Database;

namespace NewsAppBackend.Infrastructure.UseCases.GetFeed;

internal sealed class GetFeedRepository : IGetFeedRepository
{
    private readonly ReadOnlyDbContext _context;

    public GetFeedRepository(ReadOnlyDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FeedItem>> GetFeedItemsAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        var entities = await _context.FeedItems
            .OrderByDescending(f => f.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return entities.Select(entity => new FeedItem(
            entity.Id,
            entity.Title,
            entity.Content,
            entity.CreatedAt
        ));
    }
}