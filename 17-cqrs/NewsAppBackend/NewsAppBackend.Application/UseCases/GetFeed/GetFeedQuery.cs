using NewsAppBackend.Application.Common.Abstractions;

namespace NewsAppBackend.Application.UseCases.GetFeed;

public sealed record GetFeedQuery(int Page, int PageSize) : IQuery<FeedDto>;