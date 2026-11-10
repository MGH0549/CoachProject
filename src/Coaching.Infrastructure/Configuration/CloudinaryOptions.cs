
namespace Coaching.Infrastructure.Photos;

public sealed class CloudinaryOptions
{
    public string CloudName { get; set; } = "";
    public string ApiKey { get; set; } = "";
    public string ApiSecret { get; set; } = "";
    public int LinkMinutes { get; set; } = 5;
    public bool IsConfigured => !string.IsNullOrWhiteSpace(CloudName) && !string.IsNullOrWhiteSpace(ApiKey) && !string.IsNullOrWhiteSpace(ApiSecret);
}
