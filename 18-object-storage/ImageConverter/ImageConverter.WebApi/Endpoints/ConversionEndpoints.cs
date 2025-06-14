using ImageConverter.WebApi.UseCases.DownloadImage;
using ImageConverter.WebApi.UseCases.GetTaskStatus;
using ImageConverter.WebApi.UseCases.UploadImage;

namespace ImageConverter.WebApi.Endpoints;

internal static class ConversionEndpoints
{
    public static WebApplication MapConversionEndpoints(this WebApplication app)
    {
        app.MapPost("/upload", async (IFormFile file, IUploadImageHandler handler, CancellationToken cancellationToken) =>
        {
            using var fileStream = file.OpenReadStream();
            var response = await handler.HandleAsync(new UploadImageRequest(fileStream), cancellationToken);
            return Results.Ok(response);
        })
        .WithName("UploadImage")
        .WithOpenApi()
        .DisableAntiforgery();

        app.MapGet("/task/{taskId}", async (Guid taskId, IGetTaskStatusHandler handler, CancellationToken cancellationToken) =>
        {
            var response = await handler.HandleAsync(new GetTaskStatusRequest(taskId), cancellationToken);
            return Results.Ok(response);
        })
        .WithName("GetTaskStatus")
        .WithOpenApi();

        app.MapGet("/task/{taskId}/converted", async (Guid taskId, IDownloadImageHandler handler, CancellationToken cancellationToken) =>
        {
            var response = await handler.HandleAsync(new DownloadImageRequest(taskId), cancellationToken);

            return Results.File(response.Content, "image/jpeg", "converted-image.jpg");
        })
        .WithName("DownloadImage")
        .WithOpenApi();

        return app;
    }
}