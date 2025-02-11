namespace DragonSpark.Application.Security.Identity.Claims;

public readonly record struct Accessed(string Claim, bool Exists, string? Value);