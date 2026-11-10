using FluentValidation;

namespace Coaching.Application.Exercises;

public sealed class ExerciseValidator : AbstractValidator<ExerciseRequest>
{
    public ExerciseValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.MuscleGroup).NotEmpty().MaximumLength(100);
        RuleFor(x => x.YouTubeVideoId).Matches(@"^[a-zA-Z0-9_-]{11}$");
        RuleFor(x => x.Notes).MaximumLength(2000);
        RuleFor(x => x.RowVersion).Must(v => v is { Length: 8 }).When(x => x.RowVersion is not null);
    }
}
