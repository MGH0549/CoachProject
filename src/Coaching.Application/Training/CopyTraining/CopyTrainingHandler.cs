using Coaching.Domain;

namespace Coaching.Application.Training;

public sealed class CopyTrainingHandler(IStore store, SaveTrainingHandler save)
{
    public async Task<Result<TrainingResponse>> Handle(Guid templateId, CopyTemplateRequest r, CancellationToken ct)
    {
        var template = await store.Single(store.Training().Where(x => x.Id == templateId && x.IsTemplate), ct);
        if (template is null)
            return Result<TrainingResponse>.Missing();
        var current = await store.Single(store.Query<TrainingPlan>().Where(x => x.ClientId == r.ClientId), ct);
        return await save.Handle(current?.Id, template.CopyRequest(r.ClientId, r.ExistingPlanVersion), ct);
    }
}
