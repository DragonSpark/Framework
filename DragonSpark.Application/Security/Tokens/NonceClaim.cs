namespace DragonSpark.Application.Security.Tokens;

public sealed class NonceClaim : Text.Text
{
    public static NonceClaim Default { get; } = new();

    NonceClaim() : base("nonce") {}
}