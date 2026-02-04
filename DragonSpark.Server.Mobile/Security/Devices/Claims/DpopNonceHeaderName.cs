namespace DragonSpark.Server.Mobile.Security.Devices.Claims;

sealed class DpopNonceHeaderName : Text.Text
{
    public static DpopNonceHeaderName Default { get; } = new();

    DpopNonceHeaderName() : base("DPoP-Nonce") {}
}