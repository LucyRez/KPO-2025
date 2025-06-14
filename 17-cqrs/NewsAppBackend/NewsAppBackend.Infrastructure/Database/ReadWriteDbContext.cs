using Microsoft.EntityFrameworkCore;
using NewsAppBackend.Infrastructure.Database.Entities;

namespace NewsAppBackend.Infrastructure.Database;

internal sealed class ReadWriteDbContext : BaseDbContext
{
    public DbSet<DraftEntity> Drafts { get; set; }
    public DbSet<FeedItemEntity> FeedItems { get; set; }

    public ReadWriteDbContext(DbContextOptions<ReadWriteDbContext> options) : base(options)
    {
    }
}