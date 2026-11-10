namespace Coaching.Domain;

public sealed class ClientMealSelection : ClientEntity
{
    public Guid MealId { get; set; }
    public Guid? MealOptionId { get; set; }
    public bool Locked { get; set; }

    public bool CanChoose(bool coach, bool planLocked) => coach || !(Locked || planLocked);
}
