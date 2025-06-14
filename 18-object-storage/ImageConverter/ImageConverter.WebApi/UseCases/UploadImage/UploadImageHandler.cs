using Amazon.S3;
using Amazon.S3.Model;
using ImageConverter.WebApi.Configuration;
using ImageConverter.WebApi.Database;
using ImageConverter.WebApi.Models;
using Microsoft.Extensions.Options;

namespace ImageConverter.WebApi.UseCases.UploadImage;

internal sealed class UploadImageHandler : IUploadImageHandler
{
    private readonly IAmazonS3 _s3Client;
    private readonly S3Config _s3Config;
    private readonly AppDbContext _dbContext;

    public UploadImageHandler(IAmazonS3 s3Client, IOptions<S3Config> s3Config, AppDbContext dbContext)
    {
        _s3Client = s3Client;
        _s3Config = s3Config.Value;
        _dbContext = dbContext;
    }

    public async Task<UploadImageResponse> HandleAsync(UploadImageRequest request, CancellationToken cancellationToken)
    {
        var taskId = Guid.NewGuid();
        var sourceImageId = $"{taskId}-source";

        await _s3Client.PutObjectAsync(new PutObjectRequest
        {
            BucketName = _s3Config.BucketName,
            Key = sourceImageId,
            InputStream = request.FileContent,
        }, cancellationToken);
        
        var task = new ConversionTask(
            id: taskId,
            sourceImageId: sourceImageId,
            convertedImageId: null,
            status: ConversionTaskStatus.Pending,
            createdAt: DateTimeOffset.UtcNow
        );

        await _dbContext.ConversionTasks.AddAsync(task, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UploadImageResponse(taskId);
    }
}