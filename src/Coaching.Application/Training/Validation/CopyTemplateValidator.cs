using FluentValidation;

namespace Coaching.Application.Training;

public sealed class CopyTemplateValidator : AbstractValidator<CopyTemplateRequest>
{
    public CopyTemplateValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty().MaximumLength(450);
        RuleFor(x => x.ExistingPlanVersion).Must(v => v is { Length: 8 }).When(x => x.ExistingPlanVersion is not null);
    }
}
