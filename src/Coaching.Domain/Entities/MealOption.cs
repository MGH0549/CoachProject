namespace Coaching.Domain;

public sealed class MealOption : ClientEntity
{
    public Guid MealId { get; set; }
    public string Name { get; set; } = "";
    public decimal Calories { get; set; }
    public decimal Protein { get; set; }
    public decimal Carbs { get; set; }
    public decimal Fat { get; set; }
    public List<MealOptionItem> Items { get; set; } = [];
}
