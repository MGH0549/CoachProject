using Coaching.Infrastructure;
using Coaching.Infrastructure.Photos;
using Microsoft.Extensions.Options;
using Xunit;

namespace Coaching.Tests;

public sealed partial class PhotoTests
{
    [Fact]
    public async Task Unconfigured_photo_storage_returns_unavailable_without_sending_requests()
    {
        using var http = new HttpClient(new RejectingHttpTransport());
        var time = new CairoTime(TimeProvider.System, TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo"));
        var storage = new CloudinaryPhotoStorage(http, Options.Create(new CloudinaryOptions()), time);
        using var image = new MemoryStream([1, 2, 3]);
        Assert.Equal(503, (await storage.Upload("client", image, default)).Error?.Status);
        Assert.Throws<PhotoServiceException>(() => storage.SignedUrl("progress/client/photo"));
        await Assert.ThrowsAsync<PhotoServiceException>(() => storage.Delete("progress/client/photo", default));
    }
}
