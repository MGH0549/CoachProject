using Coaching.Domain;
using Xunit;

namespace Coaching.Tests;

public sealed partial class DomainTests
{
    [Fact]
    public void Activation_rejects_past_dates_and_preserves_pending_status()
    {
        var profile = new ClientProfile();
        Assert.Equal("access_end_in_past", profile.Activate("باقة", Now.AddSeconds(-1), Now));
        Assert.Equal(ClientStatus.Pending, profile.Status);
    }
}
