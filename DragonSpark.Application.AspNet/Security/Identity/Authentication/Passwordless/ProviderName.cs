namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Passwordless;

public sealed class ProviderName : Text.Text
{
    public static ProviderName Default { get; } = new();

    ProviderName() : base("PasswordlessProvider") {}
}