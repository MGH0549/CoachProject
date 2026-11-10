using Coaching.Domain;

namespace Coaching.Application.Exercises;

public sealed class SaveExerciseHandler(IStore store, IExerciseCache cache)
{
    public async Task<Result<ExerciseResponse>> Handle(Guid? id, ExerciseRequest r, CancellationToken ct)
    {
        var e = id is null ? new Exercise() : await store.Single(store.Query<Exercise>(true).Where(x => x.Id == id), ct);
        if (e is null)
            return Result<ExerciseResponse>.Missing();
        if (id is not null && r.RowVersion is null)
            return Result<ExerciseResponse>.Fail("version_required", "يجب إرسال إصدار السجل");
        if (id is null)
            store.Add(e);
        else
            store.ExpectVersion(e, r.RowVersion!);
        e.Name = r.Name;
        e.MuscleGroup = r.MuscleGroup;
        e.YouTubeVideoId = r.YouTubeVideoId;
        e.Notes = r.Notes;
        e.Archived = r.Archived;
        store.Audit("exercise.save", e.Id.ToString());
        await store.Save(ct);
        cache.Invalidate();
        return Result<ExerciseResponse>.Ok(e.Response());
    }
}
