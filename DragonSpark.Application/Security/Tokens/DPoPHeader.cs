namespace DragonSpark.Application.Security.Tokens;

public readonly record struct DPoPHeader(string Kty, string Crv, string X, string Y);