
namespace Coaching.Application;

public record Session(string AccessToken, DateTimeOffset ExpiresAt, string RefreshToken, DateTimeOffset RefreshExpiresAt);
