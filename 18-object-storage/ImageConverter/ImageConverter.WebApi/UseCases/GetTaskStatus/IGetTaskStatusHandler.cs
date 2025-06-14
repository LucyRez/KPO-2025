namespace ImageConverter.WebApi.UseCases.GetTaskStatus;

internal interface IGetTaskStatusHandler
{
    Task<GetTaskStatusResponse> HandleAsync(GetTaskStatusRequest request, CancellationToken cancellationToken);
}