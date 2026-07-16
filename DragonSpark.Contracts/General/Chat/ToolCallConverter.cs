using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DragonSpark.Contracts.General.Chat;

sealed class ToolCallConverter : JsonConverter<ToolCall>
{
    public override ToolCall Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc  = JsonDocument.ParseValue(ref reader);
        var       root = doc.RootElement;

        if (root.TryGetProperty("type", out var element))
        {
            var content = element.GetString();
            return content switch
            {
                "function" => JsonSerializer.Deserialize<FunctionToolCall>(root.GetRawText(), options)
                              ?? throw new JsonException("Failed to deserialize FunctionToolCall"),
                _ => throw new ArgumentOutOfRangeException(nameof(content))
            };
        }

        try
        {
            return JsonSerializer.Deserialize<FunctionToolCall>(root.GetRawText(), options)!;
        }
        catch
        {
            throw new JsonException("Unknown or unsupported tool call format");
        }
    }

    public override void Write(Utf8JsonWriter writer, ToolCall value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}