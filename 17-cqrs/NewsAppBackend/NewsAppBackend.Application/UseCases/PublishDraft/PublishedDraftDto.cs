namespace NewsAppBackend.Application.UseCases.PublishDraft;

public sealed record PublishedDraftDto(Guid Id, string Title, string Content, DateTimeOffset CreatedAt, PublishedFeedItemDto FeedItem);