using Coaching.Domain;

namespace Coaching.Application.Progress;

public sealed class SubmitCheckInHandler(IStore store, IActor actor, ICairoTime time, IPhotoStorage photos)
{
    public async Task<Result<CheckInResponse>> Handle(CheckInRequest r, CancellationToken ct)
    {
        var now = time.UtcNow;
        var c = new CheckIn
        {
            ClientId = actor.UserId,
            SubmittedAt = now,
            CairoDate = time.LocalDate(now),
            Weight = r.Weight,
            Waist = r.Waist,
            Chest = r.Chest,
            Hips = r.Hips,
            Energy = r.Energy,
            Notes = r.Notes
        };
        store.Add(c);
        await store.Save(ct);
        return Result<CheckInResponse>.Ok(c.Response(photos));
    }
}
