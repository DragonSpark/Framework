namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public sealed record PasskeySettings
{
    public required string Name { get; set; }
}