using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Coaching.Domain;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;

namespace Coaching.Infrastructure.Jobs;

public sealed partial class MaintenanceJobs
{
    public async Task SendEmails(CancellationToken ct)
    {
        if (!resend.Value.IsConfigured) return;
        for (var i = 0; i < 100; i++)
        {
            var found = await db.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
            {
                await using var tx = await db.Database.BeginTransactionAsync(ct);
                var now = time.UtcNow;
                var message = await db.Set<EmailMessage>().FromSqlInterpolated($"SELECT TOP(1) * FROM [EmailMessage] WITH (UPDLOCK, READPAST, ROWLOCK) WHERE [NextAttemptAt] <= {now} AND [Attempts] < 5 ORDER BY [CreatedAt]").SingleOrDefaultAsync(ct);
                if (message is null)
                    return false;
                ClientProfile? profile = null;
                if (message.ReminderForEnd is not null)
                {
                    profile = await db.Set<ClientProfile>().SingleOrDefaultAsync(x => x.ClientId == message.ClientId, ct);
                    if (profile is null || !profile.HasAccess(now) || profile.AccessEndsAt != message.ReminderForEnd)
                    {
                        db.Remove(message);
                        await db.SaveChangesAsync(ct);
                        await tx.CommitAsync(ct);
                        return true;
                    }
                }

                try
                {
                    using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.resend.com/emails");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", resend.Value.ApiKey);
                    request.Headers.Add("Idempotency-Key", "coaching/" + message.Id.ToString("N"));
                    request.Content = JsonContent.Create(new { from = resend.Value.From, to = new[] { message.Recipient }, subject = message.Subject, html = protection.CreateProtector("email-outbox").Unprotect(message.Html) });
                    using var response = await clients.CreateClient("resend").SendAsync(request, ct);
                    if (!response.IsSuccessStatusCode)
                        throw new HttpRequestException("Email delivery failed.");
                    if (profile is not null)
                        profile.ReminderSentForEnd = message.ReminderForEnd;
                    db.Remove(message);
                }
                catch (Exception ex) when (ex is HttpRequestException || ex is System.Security.Cryptography.CryptographicException || ex is OperationCanceledException && !ct.IsCancellationRequested)
                {
                    message.Attempts++;
                    message.NextAttemptAt = now.AddMinutes(Math.Pow(2, message.Attempts));
                    logger.LogWarning("Email {MessageId} delivery failed, attempt {Attempt}", message.Id, message.Attempts);
                }

                await db.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);
                return true;
            });
            if (!found)
                break;
        }
    }
}
