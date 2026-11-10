
namespace Coaching.Application.Training;

public sealed class LockExerciseHandler(IStore store)
{
    public async Task<Result<TrainingResponse>> Handle(string clientId, Guid? exerciseId, LockRequest r, CancellationToken ct)
    {
        var p = await store.Single(store.Training(true).Where(x => x.ClientId == clientId), ct);
        if (p is null)
            return Result<TrainingResponse>.Missing();
        if (exerciseId is null)
        {
            store.ExpectVersion(p, r.RowVersion);
            p.ChoicesLocked = r.Locked;
        }
        else
        {
            var e = p.Days.SelectMany(x => x.Exercises).SingleOrDefault(x => x.Id == exerciseId);
            if (e is null)
                return Result<TrainingResponse>.Missing();
            store.ExpectVersion(e, r.RowVersion);
            e.ChoiceLocked = r.Locked;
            store.ExpectVersion(p, p.RowVersion);
        }

        store.Audit("training.lock", p.Id.ToString());
        await store.Save(ct);
        return Result<TrainingResponse>.Ok(p.Response());
    }
}
