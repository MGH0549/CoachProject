using Coaching.Infrastructure;
using Coaching.Infrastructure.Photos;
using Microsoft.Extensions.Options;
using SkiaSharp;
using Xunit;

namespace Coaching.Tests;

public sealed partial class PhotoTests
{
    [Fact]
    public async Task Cloudinary_uploads_use_authenticated_assets_and_downloads_expire()
    {
        var transport = new PhotoTransport();
        var time = new CairoTime(TimeProvider.System, TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo"));
        var storage = new CloudinaryPhotoStorage(new HttpClient(transport), Options.Create(new CloudinaryOptions { CloudName = "test", ApiKey = "test-key", ApiSecret = "test-secret", LinkMinutes = 5 }), time);
        using var bitmap = new SKBitmap(2, 2);
        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = new MemoryStream(data.ToArray());
        var result = await storage.Upload("internal-id", stream, default);
        Assert.True(result.IsSuccess);
        Assert.Contains("authenticated", transport.Payload);
        var url = storage.SignedUrl(result.Value!);
        Assert.Contains("/image/download?", url);
        Assert.Contains("expires_at=", url);
        Assert.Contains("type=authenticated", url);
        Assert.Contains("signature=", url);
    }
}
