using Coaching.Domain;

namespace Coaching.Application.Clients;

public sealed class MyAccountHandler(IStore store, IActor actor, ICairoTime time)
{
    public async Task<Result<AccountResponse>> Handle(CancellationToken ct)
    {
        var p = await store.Single(store.Query<ClientProfile>().Where(x => x.ClientId == actor.UserId), ct);
        return p is null ? Result<AccountResponse>.Missing() : Result<AccountResponse>.Ok(p.Response(time.UtcNow));
    }
}
