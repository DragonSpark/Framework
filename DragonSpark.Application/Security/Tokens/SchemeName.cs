namespace DragonSpark.Application.Security.Tokens;

public sealed class SchemeName : Text.Text
{
    public static SchemeName Default { get; } = new();

    SchemeName() : base("DevicePoP") {}
}