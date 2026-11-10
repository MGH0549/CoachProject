using Coaching.Domain;

namespace Coaching.Application.Exercises;

public sealed class DeleteExerciseHandler(IStore store, IExerciseCache cache)
{
    public async Task<Result<Done>> Handle(Guid id, byte[] version, CancellationToken ct)
    {
        var e = await store.Single(store.Query<Exercise>(true).Where(x => x.Id == id), ct);
        if (e is null)
            return Result<Done>.Missing();
        if (await store.Count(store.Query<PlanExercise>().Where(x => x.ExerciseId == id), ct) > 0 || await store.Count(store.Query<ExerciseAlternative>().Where(x => x.ExerciseId == id), ct) > 0 || await store.Count(store.Query<WorkoutLog>().Where(x => x.MainExerciseId == id || x.PerformedExerciseId == id), ct) > 0)
            return Result<Done>.Fail("exercise_in_use", "التمرين مستخدم؛ يمكنك أرشفته بدلاً من حذفه", 409);
        store.ExpectVersion(e, version);
        store.Remove(e);
        store.Audit("exercise.delete", id.ToString());
        await store.Save(ct);
        cache.Invalidate();
        return Result<Done>.Ok(new());
    }
}
