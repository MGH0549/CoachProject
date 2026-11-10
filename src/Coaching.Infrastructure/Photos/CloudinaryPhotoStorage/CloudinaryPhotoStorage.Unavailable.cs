using Coaching.Application;

namespace Coaching.Infrastructure.Photos;

public sealed partial class CloudinaryPhotoStorage
{
    private static Result<T> Unavailable<T>() => Result<T>.Fail("photo_service_unavailable", "تعذر رفع الصورة. تم الاحتفاظ بالمتابعة ويمكنك إضافة الصور لاحقاً", 503);
}
