using Coaching.Domain;

namespace Coaching.Application.Diet;

public sealed class SaveDietHandler(IStore store)
{
    public async Task<Result<DietResponse>> Handle(Guid? id, DietRequest r, CancellationToken ct)
    {
        if (!r.IsTemplate && await store.Count(store.Query<ClientProfile>().Where(x => x.ClientId == r.ClientId), ct) == 0)
            return Result<DietResponse>.Missing();
        var p = id is null ? new DietPlan
        {
            ClientId = r.ClientId,
            IsTemplate = r.IsTemplate
        }

        : await store.Single(store.Diet(true).Where(x => x.Id == id), ct);
        if (p is null)
            return Result<DietResponse>.Missing();
        if (p.ClientId != r.ClientId || p.IsTemplate != r.IsTemplate)
            return Result<DietResponse>.Fail("plan_owner_immutable", "لا يمكن تغيير مالك الخطة");
        if (id is not null && r.RowVersion is null)
            return Result<DietResponse>.Fail("version_required", "يجب إرسال إصدار الخطة");
        if (id is null && !r.IsTemplate && await store.Count(store.Query<DietPlan>().Where(x => x.ClientId == r.ClientId), ct) > 0)
            return Result<DietResponse>.Fail("plan_exists", "لدى العميل خطة بالفعل", 409);
        if (id is null)
            store.Add(p);
        else
        {
            store.ExpectVersion(p, r.RowVersion!);
            foreach (var m in p.Meals.ToList())
                store.Remove(m);
            p.Meals.Clear();
        }

        p.Name = r.Name;
        p.Calories = r.Calories;
        p.Protein = r.Protein;
        p.Carbs = r.Carbs;
        p.Fat = r.Fat;
        p.ChoicesLocked = r.ChoicesLocked;
        foreach (var (input, order) in r.Meals.Select((x, i) => (x, i)))
        {
            var m = new Meal
            {
                ClientId = p.ClientId,
                DietPlanId = p.Id,
                Name = input.Name,
                Time = input.Time,
                Order = order
            };
            m.Options = input.Options.Select(o =>
            {
                var option = new MealOption
                {
                    ClientId = p.ClientId,
                    MealId = m.Id,
                    Name = o.Name,
                    Calories = o.Calories,
                    Protein = o.Protein,
                    Carbs = o.Carbs,
                    Fat = o.Fat
                };
                option.Items = o.Items.Select(i => new MealOptionItem { ClientId = p.ClientId, MealOptionId = option.Id, Food = i.Food, Quantity = i.Quantity, Unit = i.Unit }).ToList();
                return option;
            }).ToList();
            m.Selection = new ClientMealSelection
            {
                ClientId = p.ClientId,
                MealId = m.Id,
                MealOptionId = m.Options[input.SelectedOption].Id,
                Locked = input.Locked
            };
            p.Meals.Add(m);
            store.Add(m);
        }

        store.Audit("diet.save", p.Id.ToString());
        await store.Save(ct);
        return Result<DietResponse>.Ok(p.Response());
    }
}
