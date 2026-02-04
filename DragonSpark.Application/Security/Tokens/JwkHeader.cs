namespace DragonSpark.Application.Security.Tokens;

public sealed record JwkHeader(string Kty, string Crv, string X, string Y);