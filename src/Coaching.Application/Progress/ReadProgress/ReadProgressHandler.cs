using Coaching.Domain;

namespace Coaching.Application.Progress;

public sealed class ReadProgressHandler(IStore store, IPhotoStorage photos)
{
    public async Task<ProgressResponse> Handle(string clientId, int page, int size, DateOnly from, DateOnly to, CancellationToken ct)
    {
        var checks = store.CheckIns().Where(x => x.ClientId == clientId && x.CairoDate >= from && x.CairoDate <= to);
        var workouts = store.Query<WorkoutLog>().Where(x => x.ClientId == clientId && x.CairoDate >= from && x.CairoDate <= to);
        var checkCount = await store.Count(checks, ct);
        var workoutCount = await store.Count(workouts, ct);
        var c = await store.List(checks.OrderByDescending(x => x.SubmittedAt).ThenBy(x => x.Id).Skip((page - 1) * size).Take(size), ct);
        var w = await store.List(workouts.OrderByDescending(x => x.CompletedAt).ThenBy(x => x.Id).Skip((page - 1) * size).Take(size), ct);
        var totals = await store.List(workouts.GroupBy(x => x.CairoDate).OrderBy(g => g.Key).Select(g => new DayTotal(g.Key, g.Count())), ct);
        return new(new(c.Select(x => x.Response(photos)).ToList(), checkCount, page, size), new(w.Select(x => x.Response()).ToList(), workoutCount, page, size), totals);
    }
}
