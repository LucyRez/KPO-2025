using NewsAppBackend.Application.Common.Abstractions;
using NewsAppBackend.Domain.Entities;

namespace NewsAppBackend.Application.UseCases.CreateDraft;

internal sealed class CreateDraftCommandHandler(
    ICreateDraftRepository repository
) : ICommandHandler<CreateDraftCommand, CreatedDraftDto>
{
    public async Task<CreatedDraftDto> HandleAsync(CreateDraftCommand command, CancellationToken cancellationToken)
    {
        var draft = new Draft(
            Guid.NewGuid(),
            command.Title,
            command.Content,
            createdAt: DateTimeOffset.UtcNow,
            feedItem: null
        );

        var createdDraft = await repository.CreateAsync(draft, cancellationToken);

        return new CreatedDraftDto(createdDraft.Id, createdDraft.Title, createdDraft.Content, createdDraft.CreatedAt);
    }
}