using System.Collections.Generic;
using System.Text.Json.Serialization;
using DragonSpark.Contracts.General;

namespace DragonSpark.Grok.Chat;

public sealed record ChatCompletionRequest(
    [property: JsonPropertyName("model")] string Model,
    [property: JsonPropertyName("messages")]
    IReadOnlyCollection<ChatMessage> Messages,
    [property: JsonPropertyName("max_tokens")]
    int? MaxTokens = null,
    [property: JsonPropertyName("temperature")]
    double? Temperature = null,

    // Optional extras you might want later
    [property: JsonPropertyName("top_p")] double? TopP = null,
    [property: JsonPropertyName("presence_penalty")]
    double? PresencePenalty = null,
    [property: JsonPropertyName("frequency_penalty")]
    double? FrequencyPenalty = null,
    [property: JsonPropertyName("stream")] bool? Stream = null
);