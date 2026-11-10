namespace Coaching.Api;

public static class RefreshCookie
{
    public const string Name = "coaching.refresh";
    public static CookieOptions Options() => new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Path = "/api/v1/auth",
        IsEssential = true
    };
}
