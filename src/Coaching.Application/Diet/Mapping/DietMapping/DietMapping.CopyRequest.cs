using Coaching.Domain;

namespace Coaching.Application.Diet;

public static partial class DietMapping
{
    public static DietRequest CopyRequest(this DietPlan p, string client, byte[]? version) => new(client, false, p.Name, p.Calories, p.Protein, p.Carbs, p.Fat, p.ChoicesLocked, p.Meals.OrderBy(x => x.Order).Select(m => new MealInput(m.Name, m.Time, m.Selection?.Locked ?? false, Math.Max(0, m.Options.FindIndex(o => o.Id == m.Selection?.MealOptionId)), m.Options.Select(o => new MealOptionInput(o.Name, o.Calories, o.Protein, o.Carbs, o.Fat, o.Items.Select(i => new FoodInput(i.Food, i.Quantity, i.Unit)).ToList())).ToList())).ToList(), version);
}
