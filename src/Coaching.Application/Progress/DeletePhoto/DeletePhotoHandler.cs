using Coaching.Domain;

namespace Coaching.Application.Progress;

public sealed class DeletePhotoHandler(IStore store, IPhotoStorage storage)
{
    public async Task<Result<Done>> Handle(string clientId, Guid id, CancellationToken ct)
    {
        var p = await store.Single(store.Query<ProgressPhoto>(true).Where(x => x.Id == id && x.ClientId == clientId), ct);
        if (p is null)
            return Result<Done>.Missing();
        await storage.Delete(p.FileReference, ct);
        store.Remove(p);
        store.Audit("photo.delete", id.ToString());
        await store.Save(ct);
        return Result<Done>.Ok(new());
    }
}
