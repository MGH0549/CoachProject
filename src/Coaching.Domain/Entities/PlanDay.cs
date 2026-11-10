namespace Coaching.Domain;

public sealed class PlanDay : ClientEntity
{
    public Guid TrainingPlanId { get; set; }
    public string Name { get; set; } = "";
    public int Order { get; set; }
    public List<PlanExercise> Exercises { get; set; } = [];
}
