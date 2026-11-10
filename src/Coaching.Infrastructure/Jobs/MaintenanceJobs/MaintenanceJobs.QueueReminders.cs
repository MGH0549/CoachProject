using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;
using Coaching.Domain;
using Microsoft.AspNetCore.DataProtection;

namespace Coaching.Infrastructure.Jobs;

public sealed partial class MaintenanceJobs
{
    public async Task QueueReminders(CancellationToken ct)
    {
        var date = time.Today.AddDays(platform.Value.ReminderLeadDays);
        var start = time.EndOfDay(date.AddDays(-1));
        var end = time.EndOfDay(date);
        var profiles = await db.Set<ClientProfile>().AsNoTracking().Where(x => x.Status == ClientStatus.Active && x.AccessEndsAt > start && x.AccessEndsAt <= end && x.ReminderSentForEnd != x.AccessEndsAt).ToListAsync(ct);
        foreach (var p in profiles)
        {
            if (await db.Set<EmailMessage>().AnyAsync(x => x.ClientId == p.ClientId && x.ReminderForEnd == p.AccessEndsAt, ct))
                continue;
            var localEnd = TimeZoneInfo.ConvertTime(p.AccessEndsAt!.Value, zone).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            var html = $"<html lang=\"ar\" dir=\"rtl\"><body><p>ينتهي اشتراكك في {localEnd} بتوقيت القاهرة.</p><p>تواصل مع المدرب لتجديد الاشتراك.</p></body></html>";
            db.Add(new EmailMessage { ClientId = p.ClientId, ReminderForEnd = p.AccessEndsAt, Recipient = p.Email, Subject = "تذكير بانتهاء الاشتراك", Html = protection.CreateProtector("email-outbox").Protect(html), CreatedAt = time.UtcNow, NextAttemptAt = time.UtcNow });
        }

        await db.SaveChangesAsync(ct);
    }
}
