using System.Net;
using System.Net.Http.Json;
using Coaching.Application.Diet;
using Coaching.Application.Exercises;
using Coaching.Application.Progress;
using Coaching.Application.Training;
using Coaching.Domain;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Coaching.Tests;

public sealed partial class IntegrationTests
{
    [SqlFact]
    public async Task Training_diet_ownership_locks_workout_logging_and_template_copies_work_together()
    {
        await using var app = new TestApiFactory();
        await app.Initialize();
        var id = await app.AddClient("one@example.test");
        var otherId = await app.AddClient("two@example.test");
        using var coach = await app.SignIn(TestApiFactory.CoachEmail);
        using var one = await app.SignIn("one@example.test");
        using var two = await app.SignIn("two@example.test");
        var e1 = await Post<ExerciseResponse>(coach, "/api/v1/coach/exercises", new ExerciseRequest("ضغط صدر", "صدر", "dQw4w9WgXcQ", null, false, null));
        var e2 = await Post<ExerciseResponse>(coach, "/api/v1/coach/exercises", new ExerciseRequest("ضغط بديل", "صدر", "dQw4w9WgXcQ", null, false, null));
        var template = await Post<TrainingResponse>(coach, "/api/v1/coach/training-plans", new TrainingRequest(null, true, "قالب", false, [new("دفع", [new(e1.Id, 3, "8-12", 90, null, null, false, null, [new(e2.Id, null, null, null)])])], null));
        var plan = await Post<TrainingResponse>(coach, $"/api/v1/coach/templates/training/{template.Id}/copy", new CopyTemplateRequest(id, null));
        var other = await Post<TrainingResponse>(coach, $"/api/v1/coach/templates/training/{template.Id}/copy", new CopyTemplateRequest(otherId, null));
        Assert.NotEqual(template.Days[0].Id, plan.Days[0].Id);
        var exercise = plan.Days[0].Exercises[0];
        var alternative = exercise.Alternatives[0];
        Assert.Equal(HttpStatusCode.NotFound, (await two.PutAsJsonAsync($"/api/v1/me/training/exercises/{exercise.Id}/selection", new ChooseExerciseRequest(alternative.Id, exercise.RowVersion))).StatusCode);
        var selected = await Put<TrainingResponse>(one, $"/api/v1/me/training/exercises/{exercise.Id}/selection", new ChooseExerciseRequest(alternative.Id, exercise.RowVersion));
        Assert.Equal(alternative.Id, selected.Days[0].Exercises[0].SelectedAlternativeId);
        var locked = await Put<TrainingResponse>(coach, $"/api/v1/coach/clients/{id}/training/lock", new LockRequest(true, selected.RowVersion));
        Assert.Equal(HttpStatusCode.Conflict, (await one.PutAsJsonAsync($"/api/v1/me/training/exercises/{exercise.Id}/selection", new ChooseExerciseRequest(null, selected.Days[0].Exercises[0].RowVersion))).StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, (await one.PostAsJsonAsync("/api/v1/me/workouts", new WorkoutRequest(exercise.Id, null))).StatusCode);
        var workout = await Post<WorkoutResponse>(one, "/api/v1/me/workouts", new WorkoutRequest(exercise.Id, alternative.Id));
        Assert.Equal(e2.Id, workout.PerformedExerciseId);
        Assert.Equal(3, workout.Sets);
        var diet = await Post<DietResponse>(coach, "/api/v1/coach/diet-plans", new DietRequest(id, false, "غذاء", 2000, 150, 200, 70, false, [new("إفطار", null, false, 0, [new("أ", 400, 30, 40, 10, [new("بيض", 2, "حبة")]), new("ب", 500, 35, 45, 15, [new("شوفان", 100, "جرام")])])], null));
        var meal = diet.Meals[0];
        Assert.Equal(HttpStatusCode.NotFound, (await two.PutAsJsonAsync($"/api/v1/me/diet/meals/{meal.Id}/selection", new ChooseMealRequest(meal.Options[1].Id, meal.RowVersion))).StatusCode);
        var chosen = await Put<DietResponse>(one, $"/api/v1/me/diet/meals/{meal.Id}/selection", new ChooseMealRequest(meal.Options[1].Id, meal.RowVersion));
        Assert.Equal(500, chosen.SelectedTotal.Calories);
        var dietLocked = await Put<DietResponse>(coach, $"/api/v1/coach/clients/{id}/diet/meals/{meal.Id}/lock", new LockRequest(true, chosen.Meals[0].RowVersion));
        Assert.Equal(HttpStatusCode.Conflict, (await one.PutAsJsonAsync($"/api/v1/me/diet/meals/{meal.Id}/selection", new ChooseMealRequest(meal.Options[0].Id, dietLocked.Meals[0].RowVersion))).StatusCode);
        var coachChoice = await Put<DietResponse>(coach, $"/api/v1/coach/clients/{id}/diet/meals/{meal.Id}/selection", new ChooseMealRequest(meal.Options[0].Id, dietLocked.Meals[0].RowVersion));
        Assert.Equal(400, coachChoice.SelectedTotal.Calories);
        Assert.Equal(HttpStatusCode.Forbidden, (await one.GetAsync("/api/v1/coach/clients")).StatusCode);
        var c = await Post<CheckInResponse>(one, "/api/v1/me/check-ins", new CheckInRequest(80, 82, null, null, 7, "متابعة"));
        Assert.Equal(HttpStatusCode.NotFound, (await two.PostAsync($"/api/v1/me/check-ins/{c.Id}/photos", new ByteArrayContent([1, 2, 3]) { Headers = { ContentType = new("image/jpeg") } })).StatusCode);
        var progress = await one.GetFromJsonAsync<ProgressResponse>("/api/v1/me/progress");
        Assert.Single(progress!.CheckIns.Items);
        Assert.Single(progress.Workouts.Items);
        var feedback = await Put<CheckInResponse>(coach, $"/api/v1/coach/clients/{id}/check-ins/{c.Id}/feedback", new FeedbackRequest("ممتاز", c.RowVersion));
        Assert.Equal("ممتاز", feedback.CoachFeedback);
        var untouched = await coach.GetFromJsonAsync<TrainingResponse>($"/api/v1/coach/templates/training/{template.Id}");
        Assert.False(untouched!.ChoicesLocked);
        // Replacing a plan preserves historical logs and removes only the old plan-exercise link.
        await Post<TrainingResponse>(coach, $"/api/v1/coach/templates/training/{template.Id}/copy", new CopyTemplateRequest(id, locked.RowVersion));
        await app.WithDb(async db =>
        {
            var log = await db.Set<WorkoutLog>().SingleAsync();
            Assert.Null(log.PlanExerciseId);
            Assert.Equal(e2.Id, log.PerformedExerciseId);
        });
    }
}
