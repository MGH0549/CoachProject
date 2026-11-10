using Coaching.Application.Exercises;
using Coaching.Domain;

namespace Coaching.Application.Training;

public static partial class TrainingMapping
{
    public static TrainingResponse Response(this TrainingPlan p) => new(p.Id, p.ClientId, p.IsTemplate, p.Name, p.ChoicesLocked, p.Days.OrderBy(x => x.Order).Select(d => new DayResponse(d.Id, d.Name, d.Order, d.Exercises.OrderBy(x => x.Order).Select(e => new PlanExerciseResponse(e.Id, e.Exercise.Response(), e.Sets, e.Reps, e.RestSeconds, e.Tempo, e.Note, e.Order, e.ChoiceLocked, e.SelectedAlternativeId, e.Alternatives.Select(a => new AlternativeResponse(a.Id, a.Exercise.Response(), a.Sets ?? e.Sets, a.Reps ?? e.Reps, a.Note)).ToList(), e.RowVersion)).ToList())).ToList(), p.RowVersion);
}
