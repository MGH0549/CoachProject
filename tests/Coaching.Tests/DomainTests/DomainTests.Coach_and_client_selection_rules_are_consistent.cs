using Coaching.Domain;
using Xunit;

namespace Coaching.Tests;

public sealed partial class DomainTests
{
    [Theory]
    [InlineData(false, false, false, true)]
    [InlineData(false, true, false, false)]
    [InlineData(false, false, true, false)]
    [InlineData(true, true, true, true)]
    public void Coach_and_client_selection_rules_are_consistent(bool coach, bool wholePlan, bool individual, bool allowed)
    {
        Assert.Equal(allowed, new PlanExercise { ChoiceLocked = individual }.CanChoose(coach, wholePlan));
        Assert.Equal(allowed, new ClientMealSelection { Locked = individual }.CanChoose(coach, wholePlan));
    }
}
