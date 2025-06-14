using NewsAppBackend.Application.Common.Abstractions;
using NewsAppBackend.Application.UseCases.CreateDraft;

namespace NewsAppBackend.WebApi.Endpoints;

public static class CreateDraft
{
    public static void MapCreateDraft(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/drafts", async (
            CreateDraftCommand command,
            ICommandHandler<CreateDraftCommand, CreatedDraftDto> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(command, cancellationToken);
            return Results.Ok(result);
        })
        .WithName("CreateDraft")
        .WithOpenApi();
    }
}