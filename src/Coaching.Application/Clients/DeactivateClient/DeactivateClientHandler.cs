using Coaching.Domain;

namespace Coaching.Application.Clients;

public sealed class DeactivateClientHandler(IStore store, ICairoTime time)
{
    public async Task<Result<AccountResponse>> Handle(string id, VersionRequest r, CancellationToken ct)
    {
        var p = await store.Single(store.Query<ClientProfile>(true).Where(x => x.ClientId == id), ct);
        if (p is null)
            return Result<AccountResponse>.Missing();
        p.Deactivate(time.UtcNow);
        store.ExpectVersion(p, r.RowVersion);
        store.Audit("client.deactivate", id);
        await store.Save(ct);
        return Result<AccountResponse>.Ok(p.Response(time.UtcNow));
    }
}
