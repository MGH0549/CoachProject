using FluentValidation;

namespace Coaching.Application.Progress;

public sealed class WorkoutValidator : AbstractValidator<WorkoutRequest>
{
    public WorkoutValidator() => RuleFor(x => x.PlanExerciseId).NotEmpty();
}
