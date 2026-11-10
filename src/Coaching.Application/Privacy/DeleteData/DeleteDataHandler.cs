using Coaching.Domain;

namespace Coaching.Application.Privacy;

public sealed class DeleteDataHandler(IStore store, IAccounts accounts, IPhotoStorage photos, ICairoTime time)
{
    public async Task<Result<Done>> Handle(string id, byte[] version, CancellationToken ct)
    {
        var p = await store.Single(store.Query<ClientProfile>(true).Where(x => x.ClientId == id), ct);
        if (p is null)
            return Result<Done>.Missing();
        store.ExpectVersion(p, version);
        p.Deactivate(time.UtcNow);
        await store.Save(ct);
        var files = await store.List(store.Query<ProgressPhoto>().Where(x => x.ClientId == id), ct);
        foreach (var file in files)
            await photos.Delete(file.FileReference, ct);
        await accounts.Delete(id, ct);
        return Result<Done>.Ok(new());
    }
}
