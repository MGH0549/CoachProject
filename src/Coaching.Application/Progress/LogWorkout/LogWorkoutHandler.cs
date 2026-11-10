using Coaching.Domain;

namespace Coaching.Application.Progress;

public sealed class LogWorkoutHandler(IStore store, IActor actor, ICairoTime time)
{
    public async Task<Result<WorkoutResponse>> Handle(WorkoutRequest r, CancellationToken ct)
    {
        var p = await store.Single(store.Training().Where(x => x.ClientId == actor.UserId), ct);
        var e = p?.Days.SelectMany(x => x.Exercises).SingleOrDefault(x => x.Id == r.PlanExerciseId);
        if (p is null || e is null)
            return Result<WorkoutResponse>.Missing();
        var a = e.Alternatives.SingleOrDefault(x => x.Id == r.AlternativeId);
        if (r.AlternativeId is not null && a is null)
            return Result<WorkoutResponse>.Fail("invalid_alternative", "البديل لا ينتمي لهذا التمرين");
        if (!e.CanChoose(false, p.ChoicesLocked) && r.AlternativeId != e.SelectedAlternativeId)
            return Result<WorkoutResponse>.Fail("choice_locked", "يجب استخدام الاختيار الذي حدده المدرب", 409);
        var now = time.UtcNow;
        var log = new WorkoutLog
        {
            ClientId = actor.UserId,
            PlanExerciseId = e.Id,
            MainExerciseId = e.ExerciseId,
            PerformedExerciseId = a?.ExerciseId ?? e.ExerciseId,
            Sets = a?.Sets ?? e.Sets,
            Reps = a?.Reps ?? e.Reps,
            CompletedAt = now,
            CairoDate = time.LocalDate(now)
        };
        store.Add(log);
        await store.Save(ct);
        return Result<WorkoutResponse>.Ok(log.Response());
    }
}
