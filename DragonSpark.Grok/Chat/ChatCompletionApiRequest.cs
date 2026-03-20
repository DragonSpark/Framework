using System.Collections.Generic;
using System.Text.Json.Serialization;
using DragonSpark.Contracts.General.Chat;

namespace DragonSpark.Grok.Chat;

public sealed record ChatCompletionApiRequest(
    string Model,
    IReadOnlyCollection<ChatMessage> Messages,
    int? MaxTokens = null,
    double? Temperature = null,
    // Tool calling support (new fields)
    IReadOnlyList<Tool>? Tools = null, // null = no tools
    string? ToolChoice = "auto",       // "auto", "none", or specific tool name
    double? TopP = null,
    double? PresencePenalty = null,
    double? FrequencyPenalty = null,
    bool? Stream = null);

[method: JsonConstructor]
public sealed record Tool(string Type, FunctionDefinition Function)
{
    public Tool(FunctionDefinition Function) : this("function", Function) {}
}

public sealed record FunctionDefinition(string Name, string Description, FunctionParameters Parameters);

[method: JsonConstructor]
public sealed record FunctionParameters(string Type, Dictionary<string, ParameterSchema> Properties, string[] Required)
{
    public FunctionParameters(Dictionary<string, ParameterSchema> Properties, string[] Required)
        : this("object", Properties, Required) {}
}

[method: JsonConstructor]
public sealed record ParameterSchema(
    string Type,
    string Description,
    string[]? Enum = null,
    ParameterSchema? Items = null)
{
    public ParameterSchema(string Type, ParameterSchema? Items) : this(Type, string.Empty, Items) {}

    public ParameterSchema(string Type, string Description, ParameterSchema? Items)
        : this(Type, Description, null, Items) {}
}