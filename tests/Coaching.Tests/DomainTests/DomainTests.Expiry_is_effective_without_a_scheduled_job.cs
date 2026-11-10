using Coaching.Domain;
using Xunit;

namespace Coaching.Tests;

public sealed partial class DomainTests
{
    [Fact]
    public void Expiry_is_effective_without_a_scheduled_job()
    {
        var profile = new ClientProfile();
        Assert.Null(profile.Activate("باقة", Now.AddDays(1), Now));
        Assert.True(profile.HasAccess(Now));
        Assert.False(profile.HasAccess(Now.AddDays(1)));
        Assert.Equal(ClientStatus.Expired, profile.EffectiveStatus(Now.AddDays(1)));
    }
}
