namespace DragonSpark.Contracts.General.Chat;

public sealed record SystemMessage(string Content) : TextMessage(Content);