using System.Globalization;

namespace Coaching.Infrastructure.Photos;

public sealed partial class CloudinaryPhotoStorage
{
    private string Timestamp() => time.UtcNow.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
}
