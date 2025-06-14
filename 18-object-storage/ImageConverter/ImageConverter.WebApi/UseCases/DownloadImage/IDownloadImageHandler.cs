namespace ImageConverter.WebApi.UseCases.DownloadImage;

internal interface IDownloadImageHandler
{
    Task<DownloadImageResponse> HandleAsync(DownloadImageRequest request, CancellationToken cancellationToken);
}