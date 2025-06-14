using NewsAppBackend.Application.UseCases.CreateDraft;
using NewsAppBackend.Domain.Entities;
using NewsAppBackend.Infrastructure.Database;
using NewsAppBackend.Infrastructure.Database.Entities;

namespace NewsAppBackend.Infrastructure.UseCases.CreateDraft;

internal sealed class CreateDraftRepository : ICreateDraftRepository
{
    private readonly ReadWriteDbContext _context;

    public CreateDraftRepository(ReadWriteDbContext context)
    {
        _context = context;
    }

    public async Task<Draft> CreateAsync(Draft draft, CancellationToken cancellationToken)
    {
        var entity = new DraftEntity
        {
            Id = draft.Id,
            Title = draft.Title,
            Content = draft.Content,
            CreatedAt = draft.CreatedAt
        };

        _context.Drafts.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return draft;
    }
}