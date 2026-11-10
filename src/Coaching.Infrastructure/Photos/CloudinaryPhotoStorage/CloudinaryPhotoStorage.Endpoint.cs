
namespace Coaching.Infrastructure.Photos;

public sealed partial class CloudinaryPhotoStorage
{
    private string Endpoint(string action) => $"https://api.cloudinary.com/v1_1/{Uri.EscapeDataString(options.Value.CloudName)}/image/{action}";
}
