using System.Text.Json.Serialization;

namespace DragonSpark.Grok.Chat;

[method: JsonConstructor]
public sealed record Tool(string Type, FunctionDefinition Function)
{
    public Tool(FunctionDefinition Function) : this("function", Function) {}
}