using NewsAppBackend.Application.Common.Abstractions;
using NewsAppBackend.Application.UseCases.PublishDraft;

namespace NewsAppBackend.WebApi.Endpoints;

public static class PublishDraft
{
    public static void MapPublishDraft(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/drafts/{draftId}/publish", async (
            Guid draftId,
            ICommandHandler<PublishDraftCommand, PublishedDraftDto> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new PublishDraftCommand(draftId);
            var result = await handler.HandleAsync(command, cancellationToken);
            return Results.Ok(result);
        })
        .WithName("PublishDraft")
        .WithOpenApi();
    }
}