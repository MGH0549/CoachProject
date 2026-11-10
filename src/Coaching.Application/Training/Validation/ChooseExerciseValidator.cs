using FluentValidation;

namespace Coaching.Application.Training;

public sealed class ChooseExerciseValidator : AbstractValidator<ChooseExerciseRequest>
{
    public ChooseExerciseValidator() => RuleFor(x => x.RowVersion).NotNull().Must(v => v is { Length: 8 });
}
