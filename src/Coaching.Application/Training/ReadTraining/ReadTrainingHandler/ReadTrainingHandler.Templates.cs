using Coaching.Domain;

namespace Coaching.Application.Training;

public sealed partial class ReadTrainingHandler
{
    public async Task<Page<TemplateResponse>> Templates(int page, int size, CancellationToken ct)
    {
        var q = store.Query<TrainingPlan>().Where(x => x.IsTemplate);
        var count = await store.Count(q, ct);
        var data = await store.List(q.OrderBy(x => x.Name).ThenBy(x => x.Id).Skip((page - 1) * size).Take(size).Select(x => new TemplateResponse(x.Id, x.Name, x.RowVersion)), ct);
        return new(data, count, page, size);
    }
}
