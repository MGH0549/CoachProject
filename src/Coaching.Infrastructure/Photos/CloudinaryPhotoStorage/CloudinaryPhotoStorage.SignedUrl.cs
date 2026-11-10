using System.Globalization;

namespace Coaching.Infrastructure.Photos;

public sealed partial class CloudinaryPhotoStorage
{
    public string SignedUrl(string reference)
    {
        if (!options.Value.IsConfigured) throw new PhotoServiceException();
        var values = new SortedDictionary<string, string>(StringComparer.Ordinal)
        {
            ["public_id"] = reference,
            ["type"] = "authenticated",
            ["format"] = "jpg",
            ["timestamp"] = Timestamp(),
            ["expires_at"] = time.UtcNow.AddMinutes(options.Value.LinkMinutes).ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture)
        };
        var signature = Sign(values);
        values["signature"] = signature;
        values["api_key"] = options.Value.ApiKey;
        return Endpoint("download") + "?" + string.Join("&", values.Select(kv => Uri.EscapeDataString(kv.Key) + "=" + Uri.EscapeDataString(kv.Value)));
    }
}
