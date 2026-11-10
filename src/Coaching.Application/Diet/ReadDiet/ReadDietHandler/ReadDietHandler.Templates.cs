using Coaching.Application.Training;
using Coaching.Domain;

namespace Coaching.Application.Diet;

public sealed partial class ReadDietHandler
{
    public async Task<Page<TemplateResponse>> Templates(int page, int size, CancellationToken ct)
    {
        var q = store.Query<DietPlan>().Where(x => x.IsTemplate);
        var count = await store.Count(q, ct);
        var data = await store.List(q.OrderBy(x => x.Name).ThenBy(x => x.Id).Skip((page - 1) * size).Take(size).Select(x => new TemplateResponse(x.Id, x.Name, x.RowVersion)), ct);
        return new(data, count, page, size);
    }
}
