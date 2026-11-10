
namespace Coaching.Tests;

public sealed partial class TestPhotos
{
    public Task Delete(string reference, CancellationToken ct)
    {
        Deleted.Add(reference);
        return Task.CompletedTask;
    }
}
