namespace Coaching.Api;

public record SessionResponse(string AccessToken, DateTimeOffset ExpiresAt);
