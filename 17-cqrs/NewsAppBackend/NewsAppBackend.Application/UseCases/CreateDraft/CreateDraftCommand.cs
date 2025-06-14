using NewsAppBackend.Application.Common.Abstractions;

namespace NewsAppBackend.Application.UseCases.CreateDraft;

public sealed record CreateDraftCommand(string Title, string Content) : ICommand<CreatedDraftDto>;