namespace Coaching.Domain;

public sealed class Exercise : Entity
{
    public string Name { get; set; } = "";
    public string MuscleGroup { get; set; } = "";
    public string YouTubeVideoId { get; set; } = "";
    public string? Notes { get; set; }
    public bool Archived { get; set; }
}
