
namespace Coaching.Application.Diet;

public record MealOptionInput(string Name, decimal Calories, decimal Protein, decimal Carbs, decimal Fat, List<FoodInput> Items);
