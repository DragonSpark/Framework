using System.Text.Json;
using System.Text.Json.Serialization;

namespace DragonSpark.Contracts.General.Chat;

public sealed class ArgumentsStringToDictionaryConverter : JsonConverter<Dictionary<string, object?>>
{
    public override Dictionary<string, object?> Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        switch (reader)
        {
            case { TokenType: JsonTokenType.String }:
            {
                var value = reader.GetString();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    using var doc = JsonDocument.Parse(value);
                    return ConvertToDictionary(doc.RootElement);
                }

                return [];
            }
            default:
                return reader.TokenType == JsonTokenType.StartObject
                           ? ConvertToDictionary(JsonElement.ParseValue(ref reader))
                           : [];
        }
    }

    static Dictionary<string, object?> ConvertToDictionary(JsonElement element)
    {
        var result = new Dictionary<string, object?>();

        foreach (var prop in element.EnumerateObject())
        {
            result[prop.Name] = prop.Value.ValueKind switch
            {
                JsonValueKind.String => prop.Value.GetString(),
                JsonValueKind.Number => prop.Value.TryGetDecimal(out var d) ? d : prop.Value.GetDouble(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Object => ConvertToDictionary(prop.Value),
                JsonValueKind.Array => prop.Value.EnumerateArray()
                                           .Select(e => ConvertElement(e))
                                           .ToList(),
                _ => null
            };
        }

        return result;
    }

    static object? ConvertElement(JsonElement element) => element.ValueKind switch
    {
        JsonValueKind.String => element.GetString(),
        JsonValueKind.Number => element.TryGetDecimal(out var d) ? d : element.GetDouble(),
        JsonValueKind.True => true,
        JsonValueKind.False => false,
        JsonValueKind.Object => ConvertToDictionary(element),
        JsonValueKind.Array => element.EnumerateArray().Select(ConvertElement).ToList(),
        _ => null
    };

    public override void Write(Utf8JsonWriter writer, Dictionary<string, object?> value, JsonSerializerOptions options)
    {
        var json = JsonSerializer.Serialize(value, options);
        writer.WriteStringValue(json);
    }
}