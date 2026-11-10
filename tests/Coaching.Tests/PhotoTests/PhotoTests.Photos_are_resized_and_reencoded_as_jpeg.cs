using Coaching.Infrastructure.Photos;
using SkiaSharp;
using Xunit;

namespace Coaching.Tests;

public sealed partial class PhotoTests
{
    [Fact]
    public async Task Photos_are_resized_and_reencoded_as_jpeg()
    {
        using var bitmap = new SKBitmap(3000, 1000);
        bitmap.Erase(SKColors.Red);
        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = new MemoryStream(data.ToArray());
        var result = await CloudinaryPhotoStorage.CleanImage(stream, default);
        Assert.True(result.IsSuccess);
        using var cleaned = SKCodec.Create(new MemoryStream(result.Value!));
        Assert.Equal(SKEncodedImageFormat.Jpeg, cleaned.EncodedFormat);
        Assert.Equal(2048, cleaned.Info.Width);
    }
}
