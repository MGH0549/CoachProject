namespace Coaching.Domain;

public sealed class DietPlan : ClientEntity
{
    public string Name { get; set; } = "";
    public bool IsTemplate { get; set; }
    public bool ChoicesLocked { get; set; }
    public decimal Calories { get; set; }
    public decimal Protein { get; set; }
    public decimal Carbs { get; set; }
    public decimal Fat { get; set; }
    public List<Meal> Meals { get; set; } = [];
}
