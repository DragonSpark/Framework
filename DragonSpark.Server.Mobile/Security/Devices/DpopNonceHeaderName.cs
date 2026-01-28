namespace DragonSpark.Server.Mobile.Security.Devices;

sealed class DpopNonceHeaderName : Text.Text
{
    public static DpopNonceHeaderName Default { get; } = new();

    DpopNonceHeaderName() : base("DPoP-Nonce") {}
}