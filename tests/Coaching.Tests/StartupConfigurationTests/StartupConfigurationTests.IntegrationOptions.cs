using Coaching.Infrastructure;
using Coaching.Infrastructure.Jobs;
using Coaching.Infrastructure.Photos;
using Coaching.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Coaching.Tests;

public sealed partial class StartupConfigurationTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Missing_business_and_integration_settings_are_allowed_only_in_development(bool development)
    {
        var builder = Builder(development ? "Development" : "Production");
        builder.Configuration["Platform:AngularOrigin"] = "https://localhost:4200";
        builder.Services.AddInfrastructure(builder.Configuration, development);
        using var services = builder.Services.BuildServiceProvider();
        if (development)
        {
            Assert.False(services.GetRequiredService<IOptions<CloudinaryOptions>>().Value.IsConfigured);
            Assert.False(services.GetRequiredService<IOptions<ResendOptions>>().Value.IsConfigured);
            Assert.Empty(services.GetRequiredService<IOptions<PlatformOptions>>().Value.CoachWhatsApp);
        }
        else
        {
            Assert.Throws<OptionsValidationException>(() => services.GetRequiredService<IOptions<CloudinaryOptions>>().Value);
            Assert.Throws<OptionsValidationException>(() => services.GetRequiredService<IOptions<ResendOptions>>().Value);
            Assert.Throws<OptionsValidationException>(() => services.GetRequiredService<IOptions<PlatformOptions>>().Value);
        }
    }
}
