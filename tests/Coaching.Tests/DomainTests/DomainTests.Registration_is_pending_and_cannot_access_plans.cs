using Coaching.Domain;
using Xunit;

namespace Coaching.Tests;

public sealed partial class DomainTests
{
    [Fact]
    public void Registration_is_pending_and_cannot_access_plans()
    {
        var profile = new ClientProfile();
        Assert.Equal(ClientStatus.Pending, profile.Status);
        Assert.False(profile.HasAccess(Now));
    }
}
