using Coaching.Application.Clients;
using Coaching.Application.Diet;
using Coaching.Application.Progress;
using Coaching.Application.Training;
using Coaching.Domain;

namespace Coaching.Application.Privacy;

public sealed class ExportDataHandler(IStore store, ICairoTime time, IPhotoStorage photos)
{
    public async Task<Result<ExportResponse>> Handle(string id, CancellationToken ct)
    {
        var p = await store.Single(store.Query<ClientProfile>().Where(x => x.ClientId == id), ct);
        if (p is null)
            return Result<ExportResponse>.Missing();
        var training = await store.Single(store.Training().Where(x => x.ClientId == id), ct);
        var diet = await store.Single(store.Diet().Where(x => x.ClientId == id), ct);
        var checks = await store.List(store.CheckIns().Where(x => x.ClientId == id).OrderBy(x => x.SubmittedAt), ct);
        var workouts = await store.List(store.Query<WorkoutLog>().Where(x => x.ClientId == id).OrderBy(x => x.CompletedAt), ct);
        store.Audit("client.export", id);
        await store.Save(ct);
        return Result<ExportResponse>.Ok(new(p.Response(time.UtcNow), p.ConsentAt, training?.Response(), diet?.Response(), checks.Select(x => x.Response(photos)).ToList(), workouts.Select(x => x.Response()).ToList()));
    }
}
