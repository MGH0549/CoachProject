using System.Globalization;
using System.Text.Json;

namespace Coaching.Api;

public sealed partial class UtcDateTimeOffsetConverter
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        var text = reader.GetString();
        if (text is null || !text.EndsWith('Z') || !DateTimeOffset.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var date))
            throw new JsonException("UTC timestamps must end with Z.");
        return date.ToUniversalTime();
    }
}
