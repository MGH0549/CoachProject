using FluentValidation;

namespace Coaching.Application.Clients;

public sealed class ActivateValidator : AbstractValidator<ActivateRequest>
{
    public ActivateValidator()
    {
        RuleFor(x => x.Package).NotEmpty().MaximumLength(200);
        RuleFor(x => x.RowVersion).NotNull().Must(v => v is { Length: 8 });
        RuleFor(x => x.EndDate).LessThan(new DateOnly(9999, 12, 31));
    }
}
