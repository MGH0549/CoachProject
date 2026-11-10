
namespace Coaching.Infrastructure.Jobs;

public sealed partial class MaintenanceJobs
{
    public async Task RunDaily(CancellationToken ct)
    {
        var local = TimeZoneInfo.ConvertTime(time.UtcNow, zone);
        if (local.Hour >= 3)
            await RunJob("cleanup", Cleanup, ct);
        if (local.Hour >= 9)
            await RunJob("reminders", QueueReminders, ct);
    }
}
