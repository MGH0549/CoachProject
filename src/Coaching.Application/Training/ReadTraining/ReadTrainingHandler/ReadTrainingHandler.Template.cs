
namespace Coaching.Application.Training;

public sealed partial class ReadTrainingHandler
{
    public async Task<Result<TrainingResponse>> Template(Guid id, CancellationToken ct)
    {
        var p = await store.Single(store.Training().Where(x => x.Id == id && x.IsTemplate), ct);
        return p is null ? Result<TrainingResponse>.Missing() : Result<TrainingResponse>.Ok(p.Response());
    }
}
