using DragonSpark.Contracts.General.Chat;

namespace DragonSpark.Grok.Chat;

public sealed record ChatCompletionApiRequest(
    string Model,
    IReadOnlyCollection<ChatMessage> Messages,
    int? MaxTokens = null,
    double? Temperature = null,
    IReadOnlyList<Tool>? Tools = null,
    ToolChoice? ToolChoice = null,
    double? TopP = null,
    double? PresencePenalty = null,
    double? FrequencyPenalty = null,
    bool? Stream = null);