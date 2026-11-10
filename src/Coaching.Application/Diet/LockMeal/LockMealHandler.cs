using Coaching.Application.Training;

namespace Coaching.Application.Diet;

public sealed class LockMealHandler(IStore store)
{
    public async Task<Result<DietResponse>> Handle(string clientId, Guid? mealId, LockRequest r, CancellationToken ct)
    {
        var p = await store.Single(store.Diet(true).Where(x => x.ClientId == clientId), ct);
        if (p is null)
            return Result<DietResponse>.Missing();
        if (mealId is null)
        {
            store.ExpectVersion(p, r.RowVersion);
            p.ChoicesLocked = r.Locked;
        }
        else
        {
            var m = p.Meals.SingleOrDefault(x => x.Id == mealId);
            if (m?.Selection is null)
                return Result<DietResponse>.Missing();
            store.ExpectVersion(m.Selection, r.RowVersion);
            m.Selection.Locked = r.Locked;
            store.ExpectVersion(p, p.RowVersion);
        }

        store.Audit("diet.lock", p.Id.ToString());
        await store.Save(ct);
        return Result<DietResponse>.Ok(p.Response());
    }
}
