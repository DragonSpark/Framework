namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Passwordless;

public sealed class Purpose : Text.Text
{
    public static Purpose Default { get; } = new();

    Purpose() : base("passwordless-login") {}
}