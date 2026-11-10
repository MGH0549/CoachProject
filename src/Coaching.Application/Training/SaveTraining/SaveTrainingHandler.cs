using Coaching.Domain;

namespace Coaching.Application.Training;

public sealed class SaveTrainingHandler(IStore store)
{
    public async Task<Result<TrainingResponse>> Handle(Guid? id, TrainingRequest r, CancellationToken ct)
    {
        if (!r.IsTemplate && await store.Count(store.Query<ClientProfile>().Where(x => x.ClientId == r.ClientId), ct) == 0)
            return Result<TrainingResponse>.Missing();
        var p = id is null ? new TrainingPlan
        {
            ClientId = r.ClientId,
            IsTemplate = r.IsTemplate
        }

        : await store.Single(store.Training(true).Where(x => x.Id == id), ct);
        if (p is null)
            return Result<TrainingResponse>.Missing();
        if (p.ClientId != r.ClientId || p.IsTemplate != r.IsTemplate)
            return Result<TrainingResponse>.Fail("plan_owner_immutable", "لا يمكن تغيير مالك الخطة");
        if (id is not null && r.RowVersion is null)
            return Result<TrainingResponse>.Fail("version_required", "يجب إرسال إصدار الخطة");
        if (id is null && !r.IsTemplate && await store.Count(store.Query<TrainingPlan>().Where(x => x.ClientId == r.ClientId), ct) > 0)
            return Result<TrainingResponse>.Fail("plan_exists", "لدى العميل خطة بالفعل", 409);
        var ids = r.Days.SelectMany(d => d.Exercises).SelectMany(e => e.Alternatives.Select(a => a.ExerciseId).Append(e.ExerciseId)).Distinct().ToList();
        var library = await store.List(store.Query<Exercise>(true).Where(x => ids.Contains(x.Id) && !x.Archived), ct);
        if (library.Count != ids.Count)
            return Result<TrainingResponse>.Fail("invalid_exercise", "أحد التمارين غير موجود أو مؤرشف");
        var lookup = library.ToDictionary(x => x.Id);
        if (id is null)
            store.Add(p);
        else
        {
            store.ExpectVersion(p, r.RowVersion!);
            foreach (var d in p.Days.ToList())
                store.Remove(d);
            p.Days.Clear();
        }

        p.Name = r.Name;
        p.ChoicesLocked = r.ChoicesLocked;
        foreach (var (day, order) in r.Days.Select((x, i) => (x, i)))
        {
            var d = new PlanDay
            {
                ClientId = p.ClientId,
                TrainingPlanId = p.Id,
                Name = day.Name,
                Order = order
            };
            foreach (var (input, index) in day.Exercises.Select((x, i) => (x, i)))
            {
                var e = new PlanExercise
                {
                    ClientId = p.ClientId,
                    PlanDayId = d.Id,
                    ExerciseId = input.ExerciseId,
                    Exercise = lookup[input.ExerciseId],
                    Sets = input.Sets,
                    Reps = input.Reps,
                    RestSeconds = input.RestSeconds,
                    Tempo = input.Tempo,
                    Note = input.Note,
                    Order = index,
                    ChoiceLocked = input.ChoiceLocked
                };
                e.Alternatives = input.Alternatives.Select(a => new ExerciseAlternative { ClientId = p.ClientId, PlanExerciseId = e.Id, ExerciseId = a.ExerciseId, Exercise = lookup[a.ExerciseId], Sets = a.Sets, Reps = a.Reps, Note = a.Note }).ToList();
                e.SelectedAlternativeId = input.SelectedAlternative is { } selected ? e.Alternatives[selected].Id : null;
                d.Exercises.Add(e);
            }

            p.Days.Add(d);
            store.Add(d);
        }

        store.Audit("training.save", p.Id.ToString());
        await store.Save(ct);
        return Result<TrainingResponse>.Ok(p.Response());
    }
}
