
namespace Coaching.Application;

public record Page<T>(IReadOnlyList<T> Items, int Total, int Number, int Size);
