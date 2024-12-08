namespace DragonSpark.Application.Security.Identity.Claims.Access;

public readonly record struct Accessed(string Claim, bool Exists, string? Value);