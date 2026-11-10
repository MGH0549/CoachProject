using System.Net;
using System.Net.Http.Json;
using Coaching.Api;
using Coaching.Application.Clients;
using Coaching.Application.Privacy;
using Coaching.Application.Progress;
using Coaching.Domain;
using Coaching.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Coaching.Tests;

public sealed partial class IntegrationTests
{
    [SqlFact]
    public async Task Clients_can_export_and_delete_their_records_and_private_photos()
    {
        await using var app = new TestApiFactory(); await app.Initialize(); var id = await app.AddClient("privacy@example.test");
        using var client = await app.SignIn("privacy@example.test");
        var check = await Post<CheckInResponse>(client, "/api/v1/me/check-ins", new CheckInRequest(80, 90, null, null, 7, "متابعة"));
        using var upload = await client.PostAsync($"/api/v1/me/check-ins/{check.Id}/photos", new ByteArrayContent([1, 2, 3]) { Headers = { ContentType = new("image/jpeg") } });
        Assert.True(upload.IsSuccessStatusCode, await upload.Content.ReadAsStringAsync());
        var export = await client.GetFromJsonAsync<ExportResponse>("/api/v1/me/export", TestJson.Options);
        Assert.Equal(id, export!.Account.Id); Assert.Single(export.CheckIns); Assert.Single(export.CheckIns[0].Photos);
        var account = await client.GetFromJsonAsync<MyAccountResponse>("/api/v1/me", TestJson.Options);
        using var request = new HttpRequestMessage(HttpMethod.Delete, "/api/v1/me") { Content = JsonContent.Create(new VersionRequest(account!.Account.RowVersion)) };
        using var deleted = await client.SendAsync(request); Assert.True(deleted.IsSuccessStatusCode, await deleted.Content.ReadAsStringAsync());
        Assert.Single(app.Photos.Deleted); Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync("/api/v1/me")).StatusCode);
        await app.WithDb(async db =>
        {
            Assert.False(await db.Set<AppUser>().AnyAsync(x => x.Id == id)); Assert.Empty(await db.Set<CheckIn>().ToListAsync());
            Assert.Empty(await db.Set<ProgressPhoto>().ToListAsync()); Assert.Empty(await db.Set<RefreshToken>().ToListAsync());
            Assert.False(await db.Set<AuditEntry>().AnyAsync(x => x.ActorId == id || x.SubjectClientId == id || x.RecordId == id));
        });
    }
}
