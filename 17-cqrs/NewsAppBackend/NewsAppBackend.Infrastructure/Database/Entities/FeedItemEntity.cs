namespace NewsAppBackend.Infrastructure.Database.Entities;

internal sealed class FeedItemEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}