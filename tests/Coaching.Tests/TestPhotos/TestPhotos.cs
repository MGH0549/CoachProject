using Coaching.Application;

namespace Coaching.Tests;

public sealed partial class TestPhotos : IPhotoStorage
{
    public List<string> Deleted { get; } = [];
}
