
namespace Coaching.Application.Training;

public sealed class ChooseExerciseHandler(IStore store, IActor actor)
{
    public async Task<Result<TrainingResponse>> Handle(string clientId, Guid exerciseId, ChooseExerciseRequest r, CancellationToken ct)
    {
        var p = await store.Single(store.Training(true).Where(x => x.ClientId == clientId), ct);
        var e = p?.Days.SelectMany(x => x.Exercises).SingleOrDefault(x => x.Id == exerciseId);
        if (p is null || e is null)
            return Result<TrainingResponse>.Missing();
        if (!e.CanChoose(actor.IsCoach, p.ChoicesLocked))
            return Result<TrainingResponse>.Fail("choice_locked", "الاختيار مغلق بواسطة المدرب", 409);
        if (r.AlternativeId is not null && e.Alternatives.All(x => x.Id != r.AlternativeId))
            return Result<TrainingResponse>.Fail("invalid_alternative", "البديل لا ينتمي لهذا التمرين");
        store.ExpectVersion(e, r.RowVersion);
        e.SelectedAlternativeId = r.AlternativeId;
        // Touch the plan as well so a concurrent coach replacement cannot discard a selection silently.
        store.ExpectVersion(p, p.RowVersion);
        store.Audit("training.choose", e.Id.ToString());
        await store.Save(ct);
        return Result<TrainingResponse>.Ok(p.Response());
    }
}
