namespace DragonSpark.Application.AspNet.Security.Tokens;

public readonly record struct MarkUsedInput(string Identity, NoncePurpose Purpose);