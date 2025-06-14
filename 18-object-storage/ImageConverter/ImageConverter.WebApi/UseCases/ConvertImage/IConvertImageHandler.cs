namespace ImageConverter.WebApi.UseCases.ConvertImage;

internal interface IConvertImageHandler
{
    Task<ConvertImageResponse> HandleAsync(ConvertImageRequest request, CancellationToken cancellationToken);
}