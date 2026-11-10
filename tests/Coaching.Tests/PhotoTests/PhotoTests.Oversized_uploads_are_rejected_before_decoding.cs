using Coaching.Infrastructure.Photos;
using Xunit;

namespace Coaching.Tests;

public sealed partial class PhotoTests
{
    [Fact]
    public async Task Oversized_uploads_are_rejected_before_decoding()
    {
        using var stream = new MemoryStream(new byte[CloudinaryPhotoStorage.MaxBytes + 1]);
        var result = await CloudinaryPhotoStorage.CleanImage(stream, default);
        Assert.Equal(413, result.Error?.Status);
    }
}
