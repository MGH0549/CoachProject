namespace Coaching.Domain;

public sealed class CheckIn : ClientEntity
{
    public DateTimeOffset SubmittedAt { get; set; }
    public DateOnly CairoDate { get; set; }
    public decimal Weight { get; set; }
    public decimal? Waist { get; set; }
    public decimal? Chest { get; set; }
    public decimal? Hips { get; set; }
    public int Energy { get; set; }
    public string? Notes { get; set; }
    public string? CoachFeedback { get; set; }
    public List<ProgressPhoto> Photos { get; set; } = [];
}
