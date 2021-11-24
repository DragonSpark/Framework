namespace DragonSpark.Identity.PayPal;

public sealed class PayPalClaimNamespace : Text.Text
{
	public static PayPalClaimNamespace Default { get; } = new PayPalClaimNamespace();

	PayPalClaimNamespace() : base("urn:paypal") {}
}