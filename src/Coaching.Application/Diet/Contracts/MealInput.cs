
namespace Coaching.Application.Diet;

public record MealInput(string Name, TimeOnly? Time, bool Locked, int SelectedOption, List<MealOptionInput> Options);
