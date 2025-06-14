using System.ComponentModel.DataAnnotations;

namespace ImageConverter.WebApi.Configuration;

internal sealed class S3Config
{
    [Required]
    public required string ServiceUrl { get; set; }

    [Required]
    public required string AccessKey { get; set; }

    [Required]
    public required string SecretKey { get; set; }

    [Required]
    public required string BucketName { get; set; }
}
