using NewsAppBackend.Application.Common.Abstractions;

namespace NewsAppBackend.Application.UseCases.PublishDraft;

public sealed record PublishDraftCommand(Guid DraftId) : ICommand<PublishedDraftDto>;