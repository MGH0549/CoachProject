using Coaching.Domain;

namespace Coaching.Application.Exercises;

public sealed class ListExercisesHandler(IStore store)
{
    public async Task<Page<ExerciseResponse>> Handle(int page, int size, string? search, bool archived, CancellationToken ct)
    {
        var q = store.Query<Exercise>().Where(x => archived || !x.Archived);
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(x => x.Name.Contains(search) || x.MuscleGroup.Contains(search));
        var total = await store.Count(q, ct);
        var data = await store.List(q.OrderBy(x => x.Name).ThenBy(x => x.Id).Skip((page - 1) * size).Take(size), ct);
        return new(data.Select(x => x.Response()).ToList(), total, page, size);
    }
}
