using Coaching.Domain;
using Xunit;

namespace Coaching.Tests;

public sealed partial class DomainTests
{
    [Fact]
    public void Extension_requires_a_later_date_and_deactivated_clients_require_activation()
    {
        var profile = new ClientProfile();
        profile.Activate("باقة", Now.AddDays(1), Now);
        Assert.Equal("extension_must_increase", profile.Extend(Now.AddHours(1), Now));
        Assert.Null(profile.Extend(Now.AddDays(3), Now));
        profile.Deactivate(Now);
        Assert.Equal("activation_required", profile.Extend(Now.AddDays(5), Now));
        Assert.Null(profile.Activate("باقة", Now.AddDays(5), Now));
        Assert.Null(profile.DeactivatedAt);
    }
}
