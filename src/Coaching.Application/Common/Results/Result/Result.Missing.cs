
namespace Coaching.Application;

public partial record Result<T>
{
    public static Result<T> Missing() => Fail("not_found", "البيانات المطلوبة غير موجودة", 404);
}
