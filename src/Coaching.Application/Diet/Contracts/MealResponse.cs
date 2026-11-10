
namespace Coaching.Application.Diet;

public record MealResponse(Guid Id, string Name, TimeOnly? Time, int Order, bool Locked, Guid? SelectedOptionId, List<MealOptionResponse> Options, byte[] RowVersion);
