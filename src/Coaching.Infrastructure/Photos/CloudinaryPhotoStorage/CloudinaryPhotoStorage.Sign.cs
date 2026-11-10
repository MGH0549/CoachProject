using System.Security.Cryptography;
using System.Text;

namespace Coaching.Infrastructure.Photos;

public sealed partial class CloudinaryPhotoStorage
{
    private string Sign(SortedDictionary<string, string> values) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(string.Join("&", values.Select(kv => kv.Key + "=" + kv.Value)) + options.Value.ApiSecret))).ToLowerInvariant();
}
