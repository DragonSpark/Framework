namespace DragonSpark.Application.Security.Tokens;

public readonly record struct PublicJWK(string Kty, string Crv, string X, string Y, string Jkt);