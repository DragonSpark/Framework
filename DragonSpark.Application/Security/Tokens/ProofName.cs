namespace DragonSpark.Application.Security.Tokens;

public sealed class ProofName : Text.Text
{
    public static ProofName Default { get; } = new();

    ProofName() : base("DPoP") {}
}