using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;
using Coaching.Domain;
using Xunit;
using Xunit.Abstractions;

namespace Coaching.Tests;

public sealed class LoadTests(ITestOutputHelper output)
{
    [SqlFact]
    public async Task Plan_and_progress_reads_handle_a_small_concurrent_load()
    {
        await using var app = new TestApiFactory();
        await app.Initialize();
        var id = await app.AddClient("load@example.test");
        await app.WithDb(async db =>
        {
            var library = Enumerable.Range(0, 3).Select(i => new Exercise { Name = "تمرين " + i, MuscleGroup = "صدر", YouTubeVideoId = "dQw4w9WgXcQ" }).ToList();
            db.AddRange(library);
            var plan = new TrainingPlan { ClientId = id, Name = "خطة تدريب" };
            for (var day = 0; day < 3; day++)
            {
                var d = new PlanDay { ClientId = id, TrainingPlanId = plan.Id, Name = "يوم " + day, Order = day };
                for (var exercise = 0; exercise < 6; exercise++)
                {
                    var e = new PlanExercise { ClientId = id, PlanDayId = d.Id, ExerciseId = library[0].Id, Sets = 3, Reps = "8-12", RestSeconds = 90, Order = exercise };
                    e.Alternatives = library.Skip(1).Select(x => new ExerciseAlternative { ClientId = id, PlanExerciseId = e.Id, ExerciseId = x.Id }).ToList();
                    d.Exercises.Add(e);
                }
                plan.Days.Add(d);
            }
            db.Add(plan);
            for (var week = 0; week < 12; week++)
            {
                var moment = app.Clock.Current.AddDays(-7 * week);
                db.Add(new CheckIn { ClientId = id, SubmittedAt = moment, CairoDate = DateOnly.FromDateTime(moment.DateTime), Weight = 80 + week, Energy = 7 });
                for (var log = 0; log < 18; log++)
                    db.Add(new WorkoutLog { ClientId = id, MainExerciseId = library[0].Id, PerformedExerciseId = library[0].Id, CompletedAt = moment,
                        CairoDate = DateOnly.FromDateTime(moment.DateTime), Sets = 3, Reps = "8-12" });
            }
            await db.SaveChangesAsync();
        });
        using var client = await app.SignIn("load@example.test");
        var paths = new[] { "/api/v1/me/training", "/api/v1/me/progress" };
        foreach (var path in Enumerable.Repeat(paths, 10).SelectMany(x => x))
        {
            using var warmup = await client.GetAsync(path);
            Assert.True(warmup.IsSuccessStatusCode);
        }
        var samples = new ConcurrentBag<(string Path, double Milliseconds)>();
        await Task.WhenAll(Enumerable.Range(0, 5).Select(async worker =>
        {
            for (var iteration = 0; iteration < 20; iteration++)
                foreach (var path in paths)
                {
                    var watch = Stopwatch.StartNew();
                    using var response = await client.GetAsync(path);
                    Assert.True(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
                    samples.Add((path, watch.Elapsed.TotalMilliseconds));
                }
        }));
        var metrics = paths.ToDictionary(path => path, path =>
        {
            var sorted = samples.Where(x => x.Path == path).Select(x => x.Milliseconds).Order().ToArray();
            return new { requests = sorted.Length, medianMs = Math.Round(sorted[sorted.Length / 2], 2), p95Ms = Math.Round(sorted[(int)Math.Ceiling(sorted.Length * 0.95) - 1], 2) };
        });
        var report = JsonSerializer.Serialize(new { transport = "ASP.NET TestServer with real SQL Server; excludes network and TLS", workers = 5, metrics }, new JsonSerializerOptions { WriteIndented = true });
        output.WriteLine(report);
        var root = new DirectoryInfo(AppContext.BaseDirectory);
        while (root is not null && !File.Exists(Path.Combine(root.FullName, "Coaching.slnx"))) root = root.Parent;
        Assert.NotNull(root);
        Directory.CreateDirectory(Path.Combine(root.FullName, "artifacts"));
        await File.WriteAllTextAsync(Path.Combine(root.FullName, "artifacts", "sql-load-test-summary.json"), report);
        Assert.All(metrics.Values, result => Assert.Equal(100, result.requests));
    }
}
