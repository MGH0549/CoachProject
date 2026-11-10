using FluentValidation;

namespace Coaching.Application.Progress;

public sealed class CheckInValidator : AbstractValidator<CheckInRequest>
{
    public CheckInValidator()
    {
        RuleFor(x => x.Weight).InclusiveBetween(20, 500);
        RuleFor(x => x.Waist).InclusiveBetween(10, 400).When(x => x.Waist.HasValue);
        RuleFor(x => x.Chest).InclusiveBetween(10, 400).When(x => x.Chest.HasValue);
        RuleFor(x => x.Hips).InclusiveBetween(10, 400).When(x => x.Hips.HasValue);
        RuleFor(x => x.Energy).InclusiveBetween(1, 10);
        RuleFor(x => x.Notes).MaximumLength(4000);
    }
}
