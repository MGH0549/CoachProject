using SkiaSharp;
using Coaching.Application;

namespace Coaching.Infrastructure.Photos;

public sealed partial class CloudinaryPhotoStorage
{
    public static async Task<Result<byte[]>> CleanImage(Stream content, CancellationToken ct)
    {
        using var input = new MemoryStream();
        var buffer = new byte[81920];
        int read;
        while ((read = await content.ReadAsync(buffer, ct)) > 0)
        {
            if (input.Length + read > MaxBytes)
                return Result<byte[]>.Fail("photo_too_large", "الحد الأقصى للصورة 8 ميجابايت", 413);
            await input.WriteAsync(buffer.AsMemory(0, read), ct);
        }

        input.Position = 0;
        using var codec = SKCodec.Create(input);
        if (codec is null || codec.EncodedFormat is not (SKEncodedImageFormat.Jpeg or SKEncodedImageFormat.Png or SKEncodedImageFormat.Webp) || codec.FrameCount > 1 || codec.Info.Width <= 0 || codec.Info.Height <= 0 || (long)codec.Info.Width * codec.Info.Height > 20_000_000)
            return InvalidImage();
        using var decoded = new SKBitmap(codec.Info.Width, codec.Info.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
        if (codec.GetPixels(decoded.Info, decoded.GetPixels()) != SKCodecResult.Success)
            return InvalidImage();
        var rotated = codec.EncodedOrigin is >= SKEncodedOrigin.LeftTop and <= SKEncodedOrigin.LeftBottom;
        var w = decoded.Width;
        var h = decoded.Height;
        var width = rotated ? h : w;
        var height = rotated ? w : h;
        var scale = Math.Min(1f, 2048f / Math.Max(width, height));
        using var surface = SKSurface.Create(new SKImageInfo(Math.Max(1, (int)(width * scale)), Math.Max(1, (int)(height * scale))));
        var canvas = surface.Canvas;
        canvas.Clear(SKColors.White);
        canvas.Scale(scale);
        var transform = codec.EncodedOrigin switch
        {
            SKEncodedOrigin.TopRight => Matrix(-1, 0, w, 0, 1, 0),
            SKEncodedOrigin.BottomRight => Matrix(-1, 0, w, 0, -1, h),
            SKEncodedOrigin.BottomLeft => Matrix(1, 0, 0, 0, -1, h),
            SKEncodedOrigin.LeftTop => Matrix(0, 1, 0, 1, 0, 0),
            SKEncodedOrigin.RightTop => Matrix(0, -1, h, 1, 0, 0),
            SKEncodedOrigin.RightBottom => Matrix(0, -1, h, -1, 0, w),
            SKEncodedOrigin.LeftBottom => Matrix(0, 1, 0, -1, 0, w),
            _ => SKMatrix.Identity
        };
        canvas.Concat(in transform);
        using var source = SKImage.FromBitmap(decoded);
        canvas.DrawImage(source, 0, 0, new SKSamplingOptions(SKFilterMode.Linear));
        using var output = surface.Snapshot();
        // Re-encode pixels into a new image: EXIF/GPS, comments, and original payload are discarded.
        using var data = output.Encode(SKEncodedImageFormat.Jpeg, 85);
        return Result<byte[]>.Ok(data.ToArray());
    }
}
