namespace ImageConverter.WebApi.UseCases.UploadImage;

internal interface IUploadImageHandler
{
    Task<UploadImageResponse> HandleAsync(UploadImageRequest request, CancellationToken cancellationToken);
}