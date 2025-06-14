using ImageConverter.WebApi.Models;

namespace ImageConverter.WebApi.UseCases.GetTaskStatus;

internal sealed record GetTaskStatusResponse(ConversionTaskStatus Status);