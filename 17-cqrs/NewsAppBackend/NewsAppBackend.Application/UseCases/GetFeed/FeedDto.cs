namespace NewsAppBackend.Application.UseCases.GetFeed;

public sealed record FeedDto(IReadOnlyList<FeedItemDto> Items);