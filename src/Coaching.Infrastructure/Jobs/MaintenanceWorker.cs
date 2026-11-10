using Coaching.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Coaching.Infrastructure.Jobs;

public sealed class MaintenanceWorker(IServiceScopeFactory scopes, ILogger<MaintenanceWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
        do
        {
            try
            {
                using var scope = scopes.CreateScope();
                scope.ServiceProvider.GetRequiredService<RequestActor>().IsSystem = true;
                var jobs = scope.ServiceProvider.GetRequiredService<MaintenanceJobs>();
                await jobs.RunDaily(stoppingToken);
                await jobs.SendEmails(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError("Maintenance failed ({ErrorType}); it will retry", ex.GetType().Name);
            }
        }
        while (await timer.WaitForNextTickAsync(stoppingToken));
    }
}
