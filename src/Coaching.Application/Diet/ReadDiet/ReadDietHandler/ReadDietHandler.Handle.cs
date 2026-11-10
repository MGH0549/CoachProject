
namespace Coaching.Application.Diet;

public sealed partial class ReadDietHandler
{
    public async Task<Result<DietResponse>> Handle(string clientId, CancellationToken ct)
    {
        var p = await store.Single(store.Diet().Where(x => x.ClientId == clientId && !x.IsTemplate), ct);
        return p is null ? Result<DietResponse>.Missing() : Result<DietResponse>.Ok(p.Response());
    }
}
