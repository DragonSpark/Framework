namespace DragonSpark.Contracts.General.Chat;

public sealed record Usage(int PromptTokens, int CompletionTokens, int TotalTokens);