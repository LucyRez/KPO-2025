using ImageConverter.WebApi.Database;

namespace ImageConverter.WebApi.UseCases.GetTaskStatus;

internal sealed class GetTaskStatusHandler : IGetTaskStatusHandler
{
    private readonly AppDbContext _dbContext;

    public GetTaskStatusHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetTaskStatusResponse> HandleAsync(GetTaskStatusRequest request, CancellationToken cancellationToken)
    {
        var task = await _dbContext.ConversionTasks.FindAsync([request.TaskId], cancellationToken);
        if (task == null)
        {
            throw new InvalidOperationException($"Task {request.TaskId} not found");
        }

        return new GetTaskStatusResponse(task.Status);
    }
}