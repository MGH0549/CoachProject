using FluentValidation;

namespace Coaching.Application.Training;

public sealed class LockValidator : AbstractValidator<LockRequest>
{
    public LockValidator() => RuleFor(x => x.RowVersion).NotNull().Must(v => v is { Length: 8 });
}
