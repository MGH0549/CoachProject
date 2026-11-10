using Coaching.Application.Diet;
using Coaching.Application.Training;
using Xunit;

namespace Coaching.Tests;

public sealed class ValidationTests
{
    [Fact]
    public async Task Null_plan_items_are_validation_errors()
    {
        var training = new TrainingRequest(null, true, "خطة", false, [null!], null);
        Assert.False((await new TrainingValidator().ValidateAsync(training)).IsValid);
        var diet = new DietRequest(null, true, "غذاء", 2000, 100, 200, 50, false, [null!], null);
        Assert.False((await new DietValidator().ValidateAsync(diet)).IsValid);
    }
}
