
namespace Coaching.Application;

public partial interface ICairoTime
{
    DateTimeOffset UtcNow { get; }

    DateOnly Today { get; }
}
