namespace ImageConverter.WebApi.Models;

internal sealed class ConversionTask
{
    public Guid Id { get; }
    public string SourceImageId { get; }
    public string? ConvertedImageId { get; set; }
    public ConversionTaskStatus Status { get; set; }
    public DateTimeOffset CreatedAt { get; }

    public ConversionTask(Guid id, string sourceImageId, string? convertedImageId, ConversionTaskStatus status, DateTimeOffset createdAt)
    {
        Id = id;
        SourceImageId = sourceImageId;
        ConvertedImageId = convertedImageId;
        Status = status;
        CreatedAt = createdAt;
    }
}