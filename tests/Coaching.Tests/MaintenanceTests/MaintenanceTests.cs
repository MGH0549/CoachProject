using Coaching.Infrastructure.Jobs;
using Coaching.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Coaching.Tests;

public sealed partial class MaintenanceTests
{
    private static async Task RunJobs(TestApiFactory app, Func<MaintenanceJobs, Task> action)
    {
        using var scope = app.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<RequestActor>().IsSystem = true;
        await action(scope.ServiceProvider.GetRequiredService<MaintenanceJobs>());
    }
}
