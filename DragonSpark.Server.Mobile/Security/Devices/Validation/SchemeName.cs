namespace DragonSpark.Server.Mobile.Security.Devices.Validation;

public sealed class SchemeName : Text.Text
{
    public static SchemeName Default { get; } = new();

    SchemeName() : base("ValidatedDevice") {}
}