namespace NewsAppBackend.Domain.Entities;

public sealed class FeedItem
{
    public Guid Id { get; }
    public string Title { get; }
    public string Content { get; }
    public DateTimeOffset CreatedAt { get; }

    public FeedItem(Guid id, string title, string content, DateTimeOffset createdAt)
    {
        Id = id;
        Title = title;
        Content = content;
        CreatedAt = createdAt;

        Validate();
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