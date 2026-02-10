namespace DragonSpark.Application.Security.Tokens;

public sealed class TokenName : Text.Text
{
    public static TokenName Default { get; } = new();

    TokenName() : base("DPoP-Nonce") {}
}