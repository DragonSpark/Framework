namespace DragonSpark.Contracts.General.Chat;

public sealed record ToolMessage(string? ToolCallId, string Content) : ChatMessage(Content);