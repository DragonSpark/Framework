namespace DragonSpark.Contracts.General.Chat;

public abstract record TextMessage(string Content) : ChatMessage(Content);