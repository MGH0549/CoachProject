using Coaching.Domain;

namespace Coaching.Application.Diet;

public static partial class DietMapping
{
    public static DietResponse Response(this DietPlan p)
    {
        var selected = p.Meals.SelectMany(m => m.Options.Where(o => o.Id == m.Selection?.MealOptionId)).ToList();
        return new(p.Id, p.ClientId, p.IsTemplate, p.Name, new(p.Calories, p.Protein, p.Carbs, p.Fat), new(selected.Sum(x => x.Calories), selected.Sum(x => x.Protein), selected.Sum(x => x.Carbs), selected.Sum(x => x.Fat)), p.ChoicesLocked, p.Meals.OrderBy(x => x.Order).Select(m => new MealResponse(m.Id, m.Name, m.Time, m.Order, m.Selection?.Locked ?? false, m.Selection?.MealOptionId, m.Options.Select(o => new MealOptionResponse(o.Id, o.Name, o.Calories, o.Protein, o.Carbs, o.Fat, o.Items.Select(i => new FoodResponse(i.Id, i.Food, i.Quantity, i.Unit)).ToList())).ToList(), m.Selection?.RowVersion ?? m.RowVersion)).ToList(), p.RowVersion);
    }
}
