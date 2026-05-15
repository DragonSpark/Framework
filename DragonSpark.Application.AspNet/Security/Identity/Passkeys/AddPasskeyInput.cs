namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public readonly record struct AddPasskeyInput(string Credential, string? Error);