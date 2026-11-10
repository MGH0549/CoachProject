namespace Coaching.Domain;

public sealed class MealOptionItem : ClientEntity
{
    public Guid MealOptionId { get; set; }
    public string Food { get; set; } = "";
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = "";
}
