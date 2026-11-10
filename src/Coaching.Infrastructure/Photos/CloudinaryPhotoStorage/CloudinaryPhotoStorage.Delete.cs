using System.Net.Http.Json;
using System.Text.Json;

namespace Coaching.Infrastructure.Photos;

public sealed partial class CloudinaryPhotoStorage
{
    public async Task Delete(string reference, CancellationToken ct)
    {
        if (!options.Value.IsConfigured) throw new PhotoServiceException();
        var values = new SortedDictionary<string, string>(StringComparer.Ordinal)
        {
            ["public_id"] = reference,
            ["type"] = "authenticated",
            ["invalidate"] = "true",
            ["timestamp"] = Timestamp()
        };
        var signature = Sign(values);
        values["signature"] = signature;
        values["api_key"] = options.Value.ApiKey;
        using var body = new FormUrlEncodedContent(values);
        using var response = await http.PostAsync(Endpoint("destroy"), body, ct);
        if (!response.IsSuccessStatusCode)
            throw new PhotoServiceException();
        var result = await response.Content.ReadFromJsonAsync<JsonElement>(ct);
        if (result.GetProperty("result").GetString() is not ("ok" or "not found"))
            throw new PhotoServiceException();
    }
}
