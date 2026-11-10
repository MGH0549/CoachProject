
namespace Coaching.Application.Training;

public record CopyTemplateRequest(string ClientId, byte[]? ExistingPlanVersion);
