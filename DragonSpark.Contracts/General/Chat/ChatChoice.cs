namespace DragonSpark.Contracts.General.Chat;

public sealed record ChatChoice(int Index, ChatMessage Message, string FinishReason);