using ImageConverter.WebApi.Database;
using ImageConverter.WebApi.Models;
using ImageConverter.WebApi.UseCases.ConvertImage;
using Microsoft.EntityFrameworkCore;

namespace ImageConverter.WebApi.BackgroundServices;

internal sealed class ConversionBackgroundService : BackgroundService
{
    private static readonly TimeSpan Delay = TimeSpan.FromSeconds(10);
    
    private readonly ILogger<ConversionBackgroundService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ConversionBackgroundService(ILogger<ConversionBackgroundService> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ConversionBackgroundService is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = await HandleConversionAsync(stoppingToken);

                switch (result)
                {
                    case HandlerResult.NoTasks:
                    case HandlerResult.Error:
                        await Task.Delay(Delay, stoppingToken);
                        break;
                    case HandlerResult.Converted:
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ConversionBackgroundService");
                await Task.Delay(Delay, stoppingToken);
            }
        }
    }

    private async Task<HandlerResult> HandleConversionAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        var task = await dbContext.ConversionTasks
            .OrderBy(t => t.CreatedAt)
            .FirstOrDefaultAsync(t => t.Status == ConversionTaskStatus.Pending, cancellationToken);

        if (task == null)
        {
            return HandlerResult.NoTasks;
        }

        var handler = scope.ServiceProvider.GetRequiredService<IConvertImageHandler>();

        try
        {
            var response = await handler.HandleAsync(new ConvertImageRequest(task.Id), cancellationToken);

            return response.Success ? HandlerResult.Converted : HandlerResult.Error;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in HandleConversionAsync");
            return HandlerResult.Error;
        }
    }

    private enum HandlerResult
    {
        NoTasks,
        Converted,
        Error
    }
}