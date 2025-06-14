namespace NewsAppBackend.Domain.Entities;

public sealed class Draft
{
    public Guid Id { get; }
    public string Title { get; }
    public string Content { get; }
    public DateTimeOffset CreatedAt { get; }
    public FeedItem? FeedItem { get; }

    public Draft(Guid id, string title, string content, DateTimeOffset createdAt, FeedItem? feedItem)
    {
        Id = id;
        Title = title;
        Content = content;
        CreatedAt = createdAt;
        FeedItem = feedItem;

        Validate();
    }

    public Draft Publish()
    {
        if (FeedItem is not null)
        {
            throw new InvalidOperationException("Draft is already published");
        }

        var feedItem = new FeedItem(Id, Title, Content, CreatedAt);

        return new Draft(Id, Title, Content, CreatedAt, feedItem);
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            throw new ArgumentException("Title is required");
        }

        if (string.IsNullOrWhiteSpace(Content))
        {
            throw new ArgumentException("Content is required");
        }
    }
}