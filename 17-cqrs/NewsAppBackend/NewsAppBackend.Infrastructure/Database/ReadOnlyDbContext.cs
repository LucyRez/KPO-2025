using Microsoft.EntityFrameworkCore;
using NewsAppBackend.Infrastructure.Database.Entities;

namespace NewsAppBackend.Infrastructure.Database;

internal sealed class ReadOnlyDbContext : BaseDbContext
{
    public IQueryable<FeedItemEntity> FeedItems => Set<FeedItemEntity>();

    public ReadOnlyDbContext(DbContextOptions<ReadOnlyDbContext> options) : base(options)
    {
    }
}