using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using ImageConverter.WebApi.Configuration;
using ImageConverter.WebApi.Database;
using ImageConverter.WebApi.Models;
using Microsoft.Extensions.Options;

namespace ImageConverter.WebApi.UseCases.DownloadImage;

internal sealed class DownloadImageHandler : IDownloadImageHandler
{
    private readonly IAmazonS3 _s3Client;
    private readonly S3Config _s3Config;
    private readonly AppDbContext _dbContext;

    public DownloadImageHandler(IAmazonS3 s3Client, IOptions<S3Config> s3Config, AppDbContext dbContext)
    {
        _s3Client = s3Client;
        _s3Config = s3Config.Value;
        _dbContext = dbContext;
    }

    public async Task<DownloadImageResponse> HandleAsync(DownloadImageRequest request, CancellationToken cancellationToken)
    {
        var task = await _dbContext.ConversionTasks.FindAsync([request.TaskId], cancellationToken);
        if (task == null)
        {
            throw new InvalidOperationException($"Task {request.TaskId} not found");
        }

        if (task.Status != ConversionTaskStatus.Completed)
        {
            throw new InvalidOperationException($"Task {request.TaskId} is not completed");
        }

        if (task.ConvertedImageId == null)
        {
            throw new InvalidOperationException($"Task {request.TaskId} has no converted image");
        }

        var getObjectResponse = await _s3Client.GetObjectAsync(new GetObjectRequest
        {
            BucketName = _s3Config.BucketName,
            Key = task.ConvertedImageId
        }, cancellationToken);

        if (getObjectResponse.HttpStatusCode != HttpStatusCode.OK)
        {
            throw new InvalidOperationException($"Failed to download image for task {request.TaskId}");
        }

        return new DownloadImageResponse(getObjectResponse.ResponseStream);
    }
}