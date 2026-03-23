namespace DragonSpark.Contracts.General.Chat;

public sealed record UserMessage(string Content) : TextMessage(Content);