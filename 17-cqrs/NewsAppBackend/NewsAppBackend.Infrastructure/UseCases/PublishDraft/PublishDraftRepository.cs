using Microsoft.EntityFrameworkCore;
using NewsAppBackend.Application.UseCases.PublishDraft;
using NewsAppBackend.Domain.Entities;
using NewsAppBackend.Infrastructure.Database;
using NewsAppBackend.Infrastructure.Database.Entities;

namespace NewsAppBackend.Infrastructure.UseCases.PublishDraft;

internal sealed class PublishDraftRepository : IPublishDraftRepository
{
    private readonly ReadWriteDbContext _context;

    public PublishDraftRepository(ReadWriteDbContext context)
    {
        _context = context;
    }

    public async Task<Draft> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.Drafts
            .Include(d => d.FeedItem)
            .SingleOrDefaultAsync(d => d.Id == id, cancellationToken);

        if (entity is null)
        {
            throw new InvalidOperationException($"Draft with id {id} not found");
        }

        return new Draft(
            entity.Id,
            entity.Title,
            entity.Content,
            entity.CreatedAt,
            entity.FeedItem is null ? null : new FeedItem(
                entity.FeedItem.Id,
                entity.FeedItem.Title,
                entity.FeedItem.Content,
                entity.FeedItem.CreatedAt
            )
        );
    }

    public async Task<Draft> UpdateAsync(Draft draft, CancellationToken cancellationToken)
    {
        var entity = await _context.Drafts
            .Include(d => d.FeedItem)
            .SingleOrDefaultAsync(d => d.Id == draft.Id, cancellationToken);

        if (entity is null)
        {
            throw new InvalidOperationException($"Draft with id {draft.Id} not found");
        }

        if (draft.FeedItem is not null)
        {
            var feedItemEntity = new FeedItemEntity
            {
                Id = draft.FeedItem.Id,
                Title = draft.FeedItem.Title,
                Content = draft.FeedItem.Content,
                CreatedAt = draft.FeedItem.CreatedAt
            };

            _context.FeedItems.Add(feedItemEntity);
            entity.FeedItem = feedItemEntity;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return draft;
    }
}