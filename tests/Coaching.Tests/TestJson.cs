using System.Text.Json;
using System.Text.Json.Serialization;

namespace Coaching.Tests;

public static class TestJson
{
    public static JsonSerializerOptions Options { get; } = new(JsonSerializerDefaults.Web)
    { Converters = { new JsonStringEnumConverter() } };
}
