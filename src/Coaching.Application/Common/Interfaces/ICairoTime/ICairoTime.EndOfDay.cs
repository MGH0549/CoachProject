
namespace Coaching.Application;

public partial interface ICairoTime
{
    DateTimeOffset EndOfDay(DateOnly date);
}
