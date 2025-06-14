using System.Diagnostics;
using NewsAppBackend.Application.Common.Abstractions;

namespace NewsAppBackend.Application.UseCases.PublishDraft;

internal sealed class PublishDraftCommandHandler(
    IPublishDraftRepository repository
) : ICommandHandler<PublishDraftCommand, PublishedDraftDto>
{
    public async Task<PublishedDraftDto> HandleAsync(PublishDraftCommand command, CancellationToken cancellationToken)
    {
        var draft = await repository.GetByIdAsync(command.DraftId, cancellationToken);
        
        var publishedDraft = draft.Publish();

        Debug.Assert(publishedDraft.FeedItem is not null, "Draft is not published for some reason");

        await repository.UpdateAsync(publishedDraft, cancellationToken);

        var feedItemDto = new PublishedFeedItemDto(
            publishedDraft.FeedItem.Id,
            publishedDraft.FeedItem.Title,
            publishedDraft.FeedItem.Content,
            publishedDraft.FeedItem.CreatedAt
        );

        return new PublishedDraftDto(
            publishedDraft.Id,
            publishedDraft.Title,
            publishedDraft.Content,
            publishedDraft.CreatedAt,
            feedItemDto
        );
    }
}