using FluentValidation;

namespace Coaching.Application.Clients;

public sealed class VersionValidator : AbstractValidator<VersionRequest>
{
    public VersionValidator() => RuleFor(x => x.RowVersion).NotNull().Must(v => v is { Length: 8 });
}
