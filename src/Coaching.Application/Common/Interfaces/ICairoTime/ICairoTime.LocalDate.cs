
namespace Coaching.Application;

public partial interface ICairoTime
{
    DateOnly LocalDate(DateTimeOffset utc);
}
