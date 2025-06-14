using NewsAppBackend.Domain.Entities;

namespace NewsAppBackend.Application.UseCases.CreateDraft;

public interface ICreateDraftRepository
{
    Task<Draft> CreateAsync(Draft draft, CancellationToken cancellationToken);
}