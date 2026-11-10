using Coaching.Domain;
using Xunit;

namespace Coaching.Tests;

public sealed partial class DomainTests
{
    [Fact]
    public void Photo_retention_starts_from_the_earlier_end_or_deactivation()
    {
        var profile = new ClientProfile();
        profile.Activate("باقة", Now.AddDays(1), Now);
        profile.Deactivate(Now.AddDays(2));
        Assert.Equal(Now.AddDays(1), profile.AccessEndedAt(Now.AddDays(2)));
        profile.Deactivate(Now.AddDays(3));
        Assert.Equal(Now.AddDays(2), profile.DeactivatedAt);
    }
}
