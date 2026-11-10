
namespace Coaching.Application.Diet;

public sealed partial class ReadDietHandler
{
    public async Task<Result<DietResponse>> Template(Guid id, CancellationToken ct)
    {
        var p = await store.Single(store.Diet().Where(x => x.Id == id && x.IsTemplate), ct);
        return p is null ? Result<DietResponse>.Missing() : Result<DietResponse>.Ok(p.Response());
    }
}
