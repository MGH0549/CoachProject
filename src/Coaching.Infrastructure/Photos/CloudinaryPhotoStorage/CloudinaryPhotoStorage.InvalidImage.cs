using Coaching.Application;

namespace Coaching.Infrastructure.Photos;

public sealed partial class CloudinaryPhotoStorage
{
    private static Result<byte[]> InvalidImage() => Result<byte[]>.Fail("invalid_image", "الصورة غير صالحة. استخدم JPEG أو PNG أو WebP ثابتة");
}
