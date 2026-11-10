
namespace Coaching.Application.Diet;

public record MealOptionResponse(Guid Id, string Name, decimal Calories, decimal Protein, decimal Carbs, decimal Fat, List<FoodResponse> Items);
