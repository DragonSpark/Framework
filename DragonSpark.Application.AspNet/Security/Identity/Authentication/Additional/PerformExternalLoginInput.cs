namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Additional;

public readonly record struct PerformExternalLoginInput(string Provider, string? ReturnAddress, bool Persist);