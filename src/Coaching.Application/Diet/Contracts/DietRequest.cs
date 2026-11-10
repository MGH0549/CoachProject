
namespace Coaching.Application.Diet;

public record DietRequest(string? ClientId, bool IsTemplate, string Name, decimal Calories, decimal Protein, decimal Carbs, decimal Fat, bool ChoicesLocked, List<MealInput> Meals, byte[]? RowVersion);
