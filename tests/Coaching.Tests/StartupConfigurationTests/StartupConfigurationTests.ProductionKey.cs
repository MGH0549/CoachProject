using Coaching.Api;
using Coaching.Infrastructure;
using Coaching.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Coaching.Tests;

public sealed partial class StartupConfigurationTests
{
    [Fact]
    public void Production_does_not_generate_a_signing_key_or_allow_an_empty_one()
    {
        var builder = Builder("Production"); builder.AddDevelopmentDefaults();
        Assert.Null(builder.Configuration["Jwt:Key"]);
        builder.Services.AddInfrastructure(builder.Configuration);
        using var services = builder.Services.BuildServiceProvider();
        Assert.Throws<OptionsValidationException>(() => services.GetRequiredService<IOptions<JwtOptions>>().Value);
    }
}
