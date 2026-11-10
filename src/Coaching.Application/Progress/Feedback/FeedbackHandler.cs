
namespace Coaching.Application.Progress;

public sealed class FeedbackHandler(IStore store, IPhotoStorage photos)
{
    public async Task<Result<CheckInResponse>> Handle(string clientId, Guid id, FeedbackRequest r, CancellationToken ct)
    {
        var c = await store.Single(store.CheckIns(true).Where(x => x.Id == id && x.ClientId == clientId), ct);
        if (c is null)
            return Result<CheckInResponse>.Missing();
        store.ExpectVersion(c, r.RowVersion);
        c.CoachFeedback = r.Feedback;
        store.Audit("checkin.feedback", id.ToString());
        await store.Save(ct);
        return Result<CheckInResponse>.Ok(c.Response(photos));
    }
}
