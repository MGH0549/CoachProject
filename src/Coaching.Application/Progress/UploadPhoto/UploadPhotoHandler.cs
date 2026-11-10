using Coaching.Domain;

namespace Coaching.Application.Progress;

public sealed class UploadPhotoHandler(IStore store, IActor actor, IPhotoStorage storage, ICairoTime time)
{
    public async Task<Result<PhotoResponse>> Handle(Guid checkInId, Stream content, CancellationToken ct)
    {
        var c = await store.Single(store.CheckIns(true).Where(x => x.Id == checkInId && x.ClientId == actor.UserId), ct);
        if (c is null)
            return Result<PhotoResponse>.Missing();
        if (c.Photos.Count >= 10)
            return Result<PhotoResponse>.Fail("photo_limit", "الحد الأقصى عشر صور لكل متابعة");
        var upload = await storage.Upload(c.ClientId!, content, ct);
        if (!upload.IsSuccess)
            return new(null, upload.Error);
        var p = new ProgressPhoto
        {
            ClientId = c.ClientId,
            CheckInId = c.Id,
            FileReference = upload.Value!,
            CreatedAt = time.UtcNow
        };
        store.ExpectVersion(c, c.RowVersion);
        store.Add(p);
        try
        {
            await store.Save(ct);
        }
        catch
        {
            await storage.Delete(p.FileReference, CancellationToken.None);
            throw;
        }

        return Result<PhotoResponse>.Ok(new(p.Id, storage.SignedUrl(p.FileReference), p.CreatedAt));
    }
}
