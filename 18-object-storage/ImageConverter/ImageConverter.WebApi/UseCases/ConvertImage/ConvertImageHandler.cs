using Amazon.S3;
using Amazon.S3.Model;
using ImageConverter.WebApi.Configuration;
using ImageConverter.WebApi.Database;
using ImageConverter.WebApi.Models;
using Microsoft.Extensions.Options;
using SkiaSharp;

namespace ImageConverter.WebApi.UseCases.ConvertImage;

internal sealed class ConvertImageHandler : IConvertImageHandler
{
    private readonly IAmazonS3 _s3Client;
    private readonly S3Config _s3Config;
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ConvertImageHandler> _logger;

    public ConvertImageHandler(IAmazonS3 s3Client, IOptions<S3Config> s3Config, AppDbContext dbContext, ILogger<ConvertImageHandler> logger)
    {
        _s3Client = s3Client;
        _s3Config = s3Config.Value;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<ConvertImageResponse> HandleAsync(ConvertImageRequest request, CancellationToken cancellationToken)
    {
        var task = await _dbContext.ConversionTasks.FindAsync([request.TaskId], cancellationToken);
        if (task == null)
        {
            throw new InvalidOperationException($"Task {request.TaskId} not found");
        }

        try
        {
            // Помечаем задание как обрабатываемое
            task.Status = ConversionTaskStatus.Converting;
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Скачиваем изображение из S3
            var getObjectResponse = await _s3Client.GetObjectAsync(new GetObjectRequest
            {
                BucketName = _s3Config.BucketName,
                Key = task.SourceImageId
            }, cancellationToken);

            // Конвертируем изображение
            using var sourceImage = SKImage.FromEncodedData(getObjectResponse.ResponseStream);
            using var surface = SKSurface.Create(new SKImageInfo(sourceImage.Width, sourceImage.Height));
            using var canvas = surface.Canvas;
            canvas.DrawImage(sourceImage, 0, 0);

            // Загружаем результат в S3
            var convertedImageId = $"{task.Id}-converted";
            using var convertedStream = new MemoryStream();
            using var data = surface.Snapshot().Encode(SKEncodedImageFormat.Jpeg, 90);
            data.SaveTo(convertedStream);
            convertedStream.Position = 0;

            await _s3Client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = _s3Config.BucketName,
                Key = convertedImageId,
                InputStream = convertedStream
            }, cancellationToken);

            // Сохраняем идентификатор загруженного изображения и помечаем задание как выполненное
            task.ConvertedImageId = convertedImageId;
            task.Status = ConversionTaskStatus.Completed;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ConvertImageResponse(true);
        }
        catch (Exception ex)
        {
            // В случае ошибки помечаем задание как завершившееся ошибкой
            _logger.LogError(ex, "Error converting image");
            task.Status = ConversionTaskStatus.Failed;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new ConvertImageResponse(false);
        }
    }
}