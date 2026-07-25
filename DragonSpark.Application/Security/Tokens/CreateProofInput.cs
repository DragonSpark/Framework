namespace DragonSpark.Application.Security.Tokens;

public readonly record struct CreateProofInput(HttpRequestMessage Message, string? Token);