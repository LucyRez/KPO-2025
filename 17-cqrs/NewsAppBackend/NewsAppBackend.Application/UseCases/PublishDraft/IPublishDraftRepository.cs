using NewsAppBackend.Domain.Entities;

namespace NewsAppBackend.Application.UseCases.PublishDraft;

public interface IPublishDraftRepository
{
    Task<Draft> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Draft> UpdateAsync(Draft draft, CancellationToken cancellationToken);
}