using Coaching.Domain;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Coaching.Tests;

public sealed partial class MaintenanceTests
{
    [SqlFact]
    public async Task Cleanup_removes_photos_after_seven_days_and_preserves_reactivated_clients_and_measurements()
    {
        await using var app = new TestApiFactory(); await app.Initialize();
        var expired = await app.AddClient("expired-photo@example.test"); var deactivated = await app.AddClient("deactivated-photo@example.test");
        var reactivated = await app.AddClient("reactivated-photo@example.test"); var grace = await app.AddClient("grace-photo@example.test");
        await app.WithDb(async db =>
        {
            foreach (var id in new[] { expired, deactivated, reactivated, grace })
            {
                var p = await db.Set<ClientProfile>().SingleAsync(x => x.ClientId == id);
                if (id == expired) p.AccessEndsAt = app.Clock.Current.AddDays(-8);
                else if (id == deactivated) p.Deactivate(app.Clock.Current.AddDays(-8));
                else if (id == grace) p.Deactivate(app.Clock.Current.AddDays(-6));
                else { p.Deactivate(app.Clock.Current.AddDays(-6)); p.Activate("باقة", app.Clock.Current.AddDays(20), app.Clock.Current); }
                var check = new CheckIn { ClientId = id, SubmittedAt = app.Clock.Current.AddDays(-10), CairoDate = DateOnly.FromDateTime(app.Clock.Current.DateTime.AddDays(-10)), Weight = 80, Energy = 7 };
                check.Photos.Add(new ProgressPhoto { ClientId = id, CheckInId = check.Id, FileReference = "progress/" + id + "/test", CreatedAt = check.SubmittedAt }); db.Add(check);
            }
            await db.SaveChangesAsync();
        });
        await RunJobs(app, j => j.Cleanup(default));
        Assert.Equal(2, app.Photos.Deleted.Count);
        await app.WithDb(async db => { Assert.Equal(4, await db.Set<CheckIn>().CountAsync()); Assert.Equal(2, await db.Set<ProgressPhoto>().CountAsync()); });
        await RunJobs(app, j => j.Cleanup(default)); Assert.Equal(2, app.Photos.Deleted.Count);
    }
}
