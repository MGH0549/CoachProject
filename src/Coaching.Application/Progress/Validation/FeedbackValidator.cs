using FluentValidation;

namespace Coaching.Application.Progress;

public sealed class FeedbackValidator : AbstractValidator<FeedbackRequest>
{
    public FeedbackValidator()
    {
        RuleFor(x => x.Feedback).NotEmpty().MaximumLength(4000);
        RuleFor(x => x.RowVersion).NotNull().Must(v => v is { Length: 8 });
    }
}
