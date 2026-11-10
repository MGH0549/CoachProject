
namespace Coaching.Application.Diet;

public record DietResponse(Guid Id, string? ClientId, bool IsTemplate, string Name, MacroResponse Targets, MacroResponse SelectedTotal, bool ChoicesLocked, List<MealResponse> Meals, byte[] RowVersion);
