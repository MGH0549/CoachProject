using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Coaching.Domain;

namespace Coaching.Infrastructure.Jobs;

public sealed partial class MaintenanceJobs
{
    private async Task RunJob(string name, Func<CancellationToken, Task> action, CancellationToken ct)
    {
        // A SQL session lock makes the scheduler safe if another API instance is added later.
        await db.Database.OpenConnectionAsync(ct);
        try
        {
            await using var command = db.Database.GetDbConnection().CreateCommand();
            command.CommandText = "DECLARE @r int; EXEC @r = sp_getapplock @Resource=@name, @LockMode='Exclusive', @LockOwner='Session', @LockTimeout=0; SELECT @r;";
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@name";
            parameter.Value = "coaching:" + name;
            command.Parameters.Add(parameter);
            var locked = Convert.ToInt32(await command.ExecuteScalarAsync(ct), CultureInfo.InvariantCulture) >= 0;
            if (!locked)
                return;
            try
            {
                var state = await db.Set<JobState>().SingleOrDefaultAsync(x => x.Name == name, ct);
                if (state?.LastRunDate == time.Today)
                    return;
                await action(ct);
                if (state is null)
                {
                    state = new JobState
                    {
                        Name = name
                    };
                    db.Add(state);
                }

                state.LastRunDate = time.Today;
                await db.SaveChangesAsync(ct);
            }
            finally
            {
                command.CommandText = "EXEC sp_releaseapplock @Resource=@name, @LockOwner='Session';";
                await command.ExecuteNonQueryAsync(CancellationToken.None);
            }
        }
        finally
        {
            await db.Database.CloseConnectionAsync();
        }
    }
}
