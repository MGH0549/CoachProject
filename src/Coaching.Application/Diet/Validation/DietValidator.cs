using FluentValidation;

namespace Coaching.Application.Diet;

public sealed class DietValidator : AbstractValidator<DietRequest>
{
    public DietValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ClientId).NotEmpty().MaximumLength(450).When(x => !x.IsTemplate);
        RuleFor(x => x.ClientId).Null().When(x => x.IsTemplate);
        RuleFor(x => x.RowVersion).Must(v => v is { Length: 8 }).When(x => x.RowVersion is not null);
        RuleFor(x => x.Calories).InclusiveBetween(0, 20000);
        RuleFor(x => x.Protein).InclusiveBetween(0, 2000);
        RuleFor(x => x.Carbs).InclusiveBetween(0, 5000);
        RuleFor(x => x.Fat).InclusiveBetween(0, 2000);
        RuleFor(x => x.Meals).NotNull().Must(x => x is { Count: > 0 and <= 20 });
        RuleForEach(x => x.Meals).NotNull().ChildRules(m =>
        {
            m.RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            m.RuleFor(x => x.Options).NotNull().Must(x => x is { Count: >= 2 and <= 20 });
            m.RuleFor(x => x).Must(x => x.Options is not null && x.SelectedOption >= 0 && x.SelectedOption < x.Options.Count).WithMessage("خيار الوجبة غير صالح");
            m.RuleForEach(x => x.Options).NotNull().ChildRules(o =>
            {
                o.RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
                o.RuleFor(x => x.Calories).InclusiveBetween(0, 10000);
                o.RuleFor(x => x.Protein).InclusiveBetween(0, 2000);
                o.RuleFor(x => x.Carbs).InclusiveBetween(0, 3000);
                o.RuleFor(x => x.Fat).InclusiveBetween(0, 2000);
                o.RuleFor(x => x.Items).NotNull().Must(x => x is { Count: > 0 and <= 50 });
                o.RuleForEach(x => x.Items).NotNull().ChildRules(i =>
                {
                    i.RuleFor(x => x.Food).NotEmpty().MaximumLength(200);
                    i.RuleFor(x => x.Quantity).GreaterThan(0).LessThanOrEqualTo(10000);
                    i.RuleFor(x => x.Unit).NotEmpty().MaximumLength(50);
                });
            });
        });
    }
}
