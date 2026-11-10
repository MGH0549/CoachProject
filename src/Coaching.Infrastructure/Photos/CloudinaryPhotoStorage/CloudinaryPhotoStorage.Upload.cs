using System.Net.Http.Json;
using System.Text.Json;
using Coaching.Application;

namespace Coaching.Infrastructure.Photos;

public sealed partial class CloudinaryPhotoStorage
{
    public async Task<Result<string>> Upload(string clientId, Stream content, CancellationToken ct)
    {
        if (!options.Value.IsConfigured) return Unavailable<string>();
        var image = await CleanImage(content, ct);
        if (!image.IsSuccess)
            return new(null, image.Error);
        var reference = $"progress/{clientId}/{Guid.NewGuid():N}";
        var values = new SortedDictionary<string, string>(StringComparer.Ordinal)
        {
            ["public_id"] = reference,
            ["type"] = "authenticated",
            ["overwrite"] = "false",
            ["timestamp"] = Timestamp()
        };
        using var form = new MultipartFormDataContent();
        foreach (var (key, value) in values)
            form.Add(new StringContent(value), key);
        form.Add(new StringContent(Sign(values)), "signature");
        form.Add(new StringContent(options.Value.ApiKey), "api_key");
        using var file = new ByteArrayContent(image.Value!);
        file.Headers.ContentType = new("image/jpeg");
        form.Add(file, "file", "photo.jpg");
        try
        {
            using var response = await http.PostAsync(Endpoint("upload"), form, ct);
            if (!response.IsSuccessStatusCode)
                return Unavailable<string>();
            var result = await response.Content.ReadFromJsonAsync<JsonElement>(ct);
            if (result.GetProperty("public_id").GetString() != reference || result.GetProperty("type").GetString() != "authenticated")
                return Unavailable<string>();
            return Result<string>.Ok(reference);
        }
        catch (HttpRequestException)
        {
            return Unavailable<string>();
        }
        catch (OperationCanceledException) when (!ct.IsCancellationRequested)
        {
            return Unavailable<string>();
        }
    }
}
