namespace DragonSpark.Grok.Chat;

public sealed record ToolChoice(string Type, FunctionChoice? Function = null)
{
    public static ToolChoice Auto { get; } = new("auto");

    public static ToolChoice Required { get; } = new("required");

    public static ToolChoice For(string name) => new("function", new(name));
}