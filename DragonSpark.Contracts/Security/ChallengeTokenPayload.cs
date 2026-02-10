namespace DragonSpark.Contracts.Security;

public sealed record ChallengeTokenPayload(string Challenge, long IssuedAt, long ExpiresAt, string Purpose);