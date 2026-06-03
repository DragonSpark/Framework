using System.Text.Json.Serialization;

namespace DragonSpark.Grok.Chat;

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