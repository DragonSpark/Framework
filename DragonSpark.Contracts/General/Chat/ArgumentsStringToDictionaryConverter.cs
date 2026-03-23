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
        var json = reader.GetString();
        return string.IsNullOrWhiteSpace(json) ? [] : ConvertToDictionary(JsonDocument.Parse(json).RootElement);
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
                JsonValueKind.Array => prop.Value.EnumerateArray().Select(ConvertToDictionary).ToList(),
                _ => null
            };
        }

        return result;
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<string, object?> value, JsonSerializerOptions options)
    {
        var json = JsonSerializer.Serialize(value, options);
        writer.WriteStringValue(json);
    }
}