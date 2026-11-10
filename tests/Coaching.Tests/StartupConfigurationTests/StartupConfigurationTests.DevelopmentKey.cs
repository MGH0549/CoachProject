using Coaching.Api;
using Xunit;

namespace Coaching.Tests;

public sealed partial class StartupConfigurationTests
{
    [Fact]
    public void Development_generates_a_temporary_signing_key_only_when_it_is_missing()
    {
        var first = Builder("Development"); first.AddDevelopmentDefaults();
        var second = Builder("Development"); second.AddDevelopmentDefaults();
        Assert.True(Convert.FromBase64String(first.Configuration["Jwt:Key"]!).Length >= 32);
        Assert.NotEqual(first.Configuration["Jwt:Key"], second.Configuration["Jwt:Key"]);
        var configured = Builder("Development"); configured.Configuration["Jwt:Key"] = "an-explicitly-configured-signing-key-1234";
        configured.AddDevelopmentDefaults();
        Assert.Equal("an-explicitly-configured-signing-key-1234", configured.Configuration["Jwt:Key"]);
    }
}
