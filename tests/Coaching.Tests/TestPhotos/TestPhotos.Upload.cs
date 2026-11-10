using Coaching.Application;

namespace Coaching.Tests;

public sealed partial class TestPhotos
{
    public Task<Result<string>> Upload(string clientId, Stream content, CancellationToken ct) => Task.FromResult(Result<string>.Ok("progress/" + clientId + "/" + Guid.NewGuid().ToString("N")));
}
