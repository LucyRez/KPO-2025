namespace NewsAppBackend.Application.UseCases.GetFeed;

public sealed record FeedItemDto(Guid Id, string Title, string Content, DateTimeOffset CreatedAt);