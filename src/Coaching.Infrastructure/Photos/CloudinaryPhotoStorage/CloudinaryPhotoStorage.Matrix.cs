using SkiaSharp;

namespace Coaching.Infrastructure.Photos;

public sealed partial class CloudinaryPhotoStorage
{
    private static SKMatrix Matrix(float sx, float kx, float tx, float ky, float sy, float ty) => new()
    {
        ScaleX = sx,
        SkewX = kx,
        TransX = tx,
        SkewY = ky,
        ScaleY = sy,
        TransY = ty,
        Persp2 = 1
    };
}
