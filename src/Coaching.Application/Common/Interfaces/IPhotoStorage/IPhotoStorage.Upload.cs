
namespace Coaching.Application;

public partial interface IPhotoStorage
{
    Task<Result<string>> Upload(string clientId, Stream content, CancellationToken ct);
}
