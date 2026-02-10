namespace DragonSpark.Application.AspNet.Security.Tokens;

public sealed class NonceClaim : Text.Text
{
    public static NonceClaim Default { get; } = new();

    NonceClaim() : base("nonce") {}
}