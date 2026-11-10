using FluentValidation;

namespace Coaching.Application.Training;

public sealed class TrainingValidator : AbstractValidator<TrainingRequest>
{
    public TrainingValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ClientId).NotEmpty().MaximumLength(450).When(x => !x.IsTemplate);
        RuleFor(x => x.ClientId).Null().When(x => x.IsTemplate);
        RuleFor(x => x.RowVersion).Must(v => v is { Length: 8 }).When(x => x.RowVersion is not null);
        RuleFor(x => x.Days).NotNull().Must(x => x is { Count: > 0 and <= 14 });
        RuleForEach(x => x.Days).NotNull().ChildRules(d =>
        {
            d.RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            d.RuleFor(x => x.Exercises).NotNull().Must(x => x is { Count: > 0 and <= 30 });
            d.RuleForEach(x => x.Exercises).NotNull().ChildRules(e =>
            {
                e.RuleFor(x => x.ExerciseId).NotEmpty();
                e.RuleFor(x => x.Sets).InclusiveBetween(1, 100);
                e.RuleFor(x => x.Reps).NotEmpty().MaximumLength(100);
                e.RuleFor(x => x.RestSeconds).InclusiveBetween(0, 3600);
                e.RuleFor(x => x.Tempo).MaximumLength(100);
                e.RuleFor(x => x.Note).MaximumLength(2000);
                e.RuleFor(x => x.Alternatives).NotNull().Must(x => x is { Count: <= 20 });
                e.RuleFor(x => x).Must(x => x.SelectedAlternative is null || x.Alternatives is not null && x.SelectedAlternative >= 0 && x.SelectedAlternative < x.Alternatives.Count).WithMessage("البديل المحدد غير صالح");
                e.RuleForEach(x => x.Alternatives).NotNull().ChildRules(a =>
                {
                    a.RuleFor(x => x.ExerciseId).NotEmpty();
                    a.RuleFor(x => x.Sets).InclusiveBetween(1, 100).When(x => x.Sets.HasValue);
                    a.RuleFor(x => x.Reps).MaximumLength(100);
                    a.RuleFor(x => x.Note).MaximumLength(2000);
                });
            });
        });
    }
}
