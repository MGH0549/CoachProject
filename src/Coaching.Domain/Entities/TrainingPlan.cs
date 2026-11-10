namespace Coaching.Domain;

public sealed class TrainingPlan : ClientEntity
{
    public string Name { get; set; } = "";
    public bool IsTemplate { get; set; }
    public bool ChoicesLocked { get; set; }
    public List<PlanDay> Days { get; set; } = [];
}
