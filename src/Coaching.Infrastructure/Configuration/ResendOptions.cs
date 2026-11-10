
namespace Coaching.Infrastructure.Jobs;

public sealed class ResendOptions
{
    public string ApiKey { get; set; } = "";
    public string From { get; set; } = "";
    public bool IsConfigured => !string.IsNullOrWhiteSpace(ApiKey) && !string.IsNullOrWhiteSpace(From);
}
