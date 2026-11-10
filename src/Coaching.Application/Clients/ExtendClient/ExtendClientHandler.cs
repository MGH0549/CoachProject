using Coaching.Domain;

namespace Coaching.Application.Clients;

public sealed class ExtendClientHandler(IStore store, ICairoTime time)
{
    public async Task<Result<AccountResponse>> Handle(string id, ExtendRequest r, CancellationToken ct)
    {
        var p = await store.Single(store.Query<ClientProfile>(true).Where(x => x.ClientId == id), ct);
        if (p is null)
            return Result<AccountResponse>.Missing();
        var error = p.Extend(time.EndOfDay(r.EndDate), time.UtcNow);
        if (error is not null)
            return Result<AccountResponse>.Fail(error, "يجب تفعيل الحساب أولاً واختيار تاريخ بعد تاريخ الانتهاء الحالي");
        store.ExpectVersion(p, r.RowVersion);
        store.Audit("client.extend", id);
        await store.Save(ct);
        return Result<AccountResponse>.Ok(p.Response(time.UtcNow));
    }
}
