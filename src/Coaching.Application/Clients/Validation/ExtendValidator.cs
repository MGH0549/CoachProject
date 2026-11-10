using FluentValidation;

namespace Coaching.Application.Clients;

public sealed class ExtendValidator : AbstractValidator<ExtendRequest>
{
    public ExtendValidator()
    {
        RuleFor(x => x.RowVersion).NotNull().Must(v => v is { Length: 8 });
        RuleFor(x => x.EndDate).LessThan(new DateOnly(9999, 12, 31));
    }
}
