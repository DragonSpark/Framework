namespace DragonSpark.Contracts.General.Chat;

public sealed record FunctionToolCall(string Id, FunctionCall Function) : ToolCall(Id);