namespace Coaching.Domain;

public sealed class Meal : ClientEntity
{
    public Guid DietPlanId { get; set; }
    public string Name { get; set; } = "";
    public TimeOnly? Time { get; set; }
    public int Order { get; set; }
    public List<MealOption> Options { get; set; } = [];
    public ClientMealSelection? Selection { get; set; }
}
