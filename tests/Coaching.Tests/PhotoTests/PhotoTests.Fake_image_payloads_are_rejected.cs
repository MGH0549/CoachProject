using System.Text;
using Coaching.Infrastructure.Photos;
using Xunit;

namespace Coaching.Tests;

public sealed partial class PhotoTests
{
    [Fact]
    public async Task Fake_image_payloads_are_rejected()
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("<script>alert(1)</script>"));
        var result = await CloudinaryPhotoStorage.CleanImage(stream, default);
        Assert.Equal("invalid_image", result.Error?.Code);
    }
}
