using Coaching.Domain;

namespace Coaching.Application.Progress;

public static partial class ProgressMapping
{
    public static CheckInResponse Response(this CheckIn x, IPhotoStorage storage) => new(x.Id, x.SubmittedAt, x.CairoDate, x.Weight, x.Waist, x.Chest, x.Hips, x.Energy, x.Notes, x.CoachFeedback, x.Photos.Select(p => new PhotoResponse(p.Id, storage.SignedUrl(p.FileReference), p.CreatedAt)).ToList(), x.RowVersion);
}
