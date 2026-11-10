using FluentValidation;

namespace Coaching.Application.Diet;

public sealed class ChooseMealValidator : AbstractValidator<ChooseMealRequest>
{
    public ChooseMealValidator()
    {
        RuleFor(x => x.OptionId).NotEmpty();
        RuleFor(x => x.RowVersion).NotNull().Must(v => v is { Length: 8 });
    }
}
