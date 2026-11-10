using Coaching.Infrastructure;
using Coaching.Infrastructure.Photos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Coaching.Tests;

public sealed partial class StartupConfigurationTests
{
    [Fact]
    public void Partially_configured_cloudinary_credentials_are_rejected_in_development()
    {
        var builder = Builder("Development"); builder.Configuration["Cloudinary:ApiKey"] = "incomplete-key";
        builder.Services.AddInfrastructure(builder.Configuration, true);
        using var services = builder.Services.BuildServiceProvider();
        Assert.Throws<OptionsValidationException>(() => services.GetRequiredService<IOptions<CloudinaryOptions>>().Value);
    }
}
