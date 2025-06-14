namespace NewsAppBackend.Application.UseCases.CreateDraft;

public sealed record CreatedDraftDto(Guid Id, string Title, string Content, DateTimeOffset CreatedAt);