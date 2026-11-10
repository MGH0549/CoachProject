using Coaching.Application.Training;
using Coaching.Domain;

namespace Coaching.Application.Diet;

public sealed class CopyDietHandler(IStore store, SaveDietHandler save)
{
    public async Task<Result<DietResponse>> Handle(Guid templateId, CopyTemplateRequest r, CancellationToken ct)
    {
        var template = await store.Single(store.Diet().Where(x => x.Id == templateId && x.IsTemplate), ct);
        if (template is null)
            return Result<DietResponse>.Missing();
        var current = await store.Single(store.Query<DietPlan>().Where(x => x.ClientId == r.ClientId), ct);
        return await save.Handle(current?.Id, template.CopyRequest(r.ClientId, r.ExistingPlanVersion), ct);
    }
}
