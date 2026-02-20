namespace DragonSpark.Application.Security.Tokens;

public sealed record PublicJWK(string Kty, string Crv, string X, string Y, string Jkt);