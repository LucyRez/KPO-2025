namespace NewsAppBackend.Application.UseCases.PublishDraft;

public sealed record PublishedFeedItemDto(Guid Id, string Title, string Content, DateTimeOffset CreatedAt);