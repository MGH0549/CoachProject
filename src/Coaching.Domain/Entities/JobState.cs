namespace Coaching.Domain;

public sealed class JobState : Entity
{
    public string Name { get; set; } = "";
    public DateOnly LastRunDate { get; set; }
}
