using System.Globalization;
using System.Text.Json;

namespace Coaching.Api;

public sealed partial class UtcDateTimeOffsetConverter
{
    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options) => writer.WriteStringValue(value.UtcDateTime.ToString("O", CultureInfo.InvariantCulture));
}
