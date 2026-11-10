using Coaching.Domain;

namespace Coaching.Application.Clients;

public sealed class ActivateClientHandler(IStore store, ICairoTime time)
{
    public async Task<Result<AccountResponse>> Handle(string id, ActivateRequest r, CancellationToken ct)
    {
        var p = await store.Single(store.Query<ClientProfile>(true).Where(x => x.ClientId == id), ct);
        if (p is null)
            return Result<AccountResponse>.Missing();
        var error = p.Activate(r.Package, time.EndOfDay(r.EndDate), time.UtcNow);
        if (error is not null)
            return Result<AccountResponse>.Fail(error, "يجب تحديد باقة وتاريخ انتهاء في المستقبل");
        store.ExpectVersion(p, r.RowVersion);
        store.Audit("client.activate", id);
        await store.Save(ct);
        return Result<AccountResponse>.Ok(p.Response(time.UtcNow));
    }
}
