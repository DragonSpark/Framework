namespace DragonSpark.Server.Mobile.Security.Devices;

public sealed class SchemeName : Text.Text
{
    public static SchemeName Default { get; } = new();

    SchemeName() : base("DevicePoP") {}
}