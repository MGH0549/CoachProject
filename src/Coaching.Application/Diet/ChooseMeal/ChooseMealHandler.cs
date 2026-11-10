
namespace Coaching.Application.Diet;

public sealed class ChooseMealHandler(IStore store, IActor actor)
{
    public async Task<Result<DietResponse>> Handle(string clientId, Guid mealId, ChooseMealRequest r, CancellationToken ct)
    {
        var p = await store.Single(store.Diet(true).Where(x => x.ClientId == clientId), ct);
        var m = p?.Meals.SingleOrDefault(x => x.Id == mealId);
        if (p is null || m?.Selection is null)
            return Result<DietResponse>.Missing();
        if (!m.Selection.CanChoose(actor.IsCoach, p.ChoicesLocked))
            return Result<DietResponse>.Fail("choice_locked", "الاختيار مغلق بواسطة المدرب", 409);
        if (m.Options.All(x => x.Id != r.OptionId))
            return Result<DietResponse>.Fail("invalid_option", "الخيار لا ينتمي لهذه الوجبة");
        store.ExpectVersion(m.Selection, r.RowVersion);
        m.Selection.MealOptionId = r.OptionId;
        store.ExpectVersion(p, p.RowVersion);
        store.Audit("diet.choose", mealId.ToString());
        await store.Save(ct);
        return Result<DietResponse>.Ok(p.Response());
    }
}
