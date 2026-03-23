using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using DragonSpark.Contracts.General.Chat;

namespace DragonSpark.Grok.Chat;

sealed class ToolCallConverter : JsonConverter<ToolCall>
{
    public static ToolCallConverter Default { get; } = new();

    ToolCallConverter() {}

    public override ToolCall Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc  = JsonDocument.ParseValue(ref reader);
        var       root = doc.RootElement;

        if (!root.TryGetProperty("type", out var typeProp))
            throw new JsonException("Missing type discriminator 'type'.");

        var discriminator = typeProp.GetString();

        return discriminator switch
        {
            "function" => JsonSerializer.Deserialize<FunctionToolCall>(root.GetRawText(), options)
                          ?? throw new JsonException("Failed to deserialize FunctionToolCall."),

            _ => throw new JsonException($"Unknown tool call type '{discriminator}'.")
        };
    }

    public override void Write(Utf8JsonWriter writer, ToolCall value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, options);
    }
}