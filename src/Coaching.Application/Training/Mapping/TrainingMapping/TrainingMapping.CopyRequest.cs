using Coaching.Domain;

namespace Coaching.Application.Training;

public static partial class TrainingMapping
{
    public static TrainingRequest CopyRequest(this TrainingPlan p, string clientId, byte[]? version) => new(clientId, false, p.Name, p.ChoicesLocked, p.Days.OrderBy(x => x.Order).Select(d => new DayInput(d.Name, d.Exercises.OrderBy(x => x.Order).Select(e => new PlanExerciseInput(e.ExerciseId, e.Sets, e.Reps, e.RestSeconds, e.Tempo, e.Note, e.ChoiceLocked, e.SelectedAlternativeId is null ? null : e.Alternatives.FindIndex(a => a.Id == e.SelectedAlternativeId), e.Alternatives.Select(a => new AlternativeInput(a.ExerciseId, a.Sets, a.Reps, a.Note)).ToList())).ToList())).ToList(), version);
}
