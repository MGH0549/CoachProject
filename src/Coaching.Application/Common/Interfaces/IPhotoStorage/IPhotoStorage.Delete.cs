
namespace Coaching.Application;

public partial interface IPhotoStorage
{
    Task Delete(string reference, CancellationToken ct);
}
