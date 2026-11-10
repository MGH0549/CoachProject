using Coaching.Application;
using Coaching.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Coaching.Tests;

public sealed partial class MaintenanceTests
{
    [SqlFact]
    public async Task Reminders_are_deduplicated_retried_and_sent_again_for_an_extended_end_date()
    {
        await using var app = new TestApiFactory(); await app.Initialize(); var id = await app.AddClient("reminder@example.test");
        var time = app.Services.GetRequiredService<ICairoTime>(); var end = time.EndOfDay(time.Today.AddDays(3));
        await app.WithDb(async db => { var p = await db.Set<ClientProfile>().SingleAsync(x => x.ClientId == id); p.AccessEndsAt = end; await db.SaveChangesAsync(); });
        await RunJobs(app, j => j.QueueReminders(default)); await RunJobs(app, j => j.QueueReminders(default));
        await app.WithDb(async db => Assert.Single(await db.Set<EmailMessage>().ToListAsync()));
        app.Emails.Fail = true; await RunJobs(app, j => j.SendEmails(default));
        await app.WithDb(async db => { var m = await db.Set<EmailMessage>().SingleAsync(); Assert.Equal(1, m.Attempts); Assert.True(m.NextAttemptAt > time.UtcNow); });
        app.Emails.Fail = false; app.Clock.Current = app.Clock.Current.AddMinutes(3);
        await RunJobs(app, j => j.SendEmails(default)); await RunJobs(app, j => j.SendEmails(default)); Assert.Single(app.Emails.Messages);
        Assert.Contains("dir=\"rtl\"", app.Emails.Messages[0].GetProperty("html").GetString());
        await app.WithDb(async db => { var p = await db.Set<ClientProfile>().SingleAsync(x => x.ClientId == id); Assert.Equal(end, p.ReminderSentForEnd); p.AccessEndsAt = time.EndOfDay(time.Today.AddDays(10)); await db.SaveChangesAsync(); });
        app.Clock.Current = app.Clock.Current.AddDays(7);
        await RunJobs(app, j => j.QueueReminders(default)); await RunJobs(app, j => j.SendEmails(default)); Assert.Equal(2, app.Emails.Messages.Count);
    }
}
