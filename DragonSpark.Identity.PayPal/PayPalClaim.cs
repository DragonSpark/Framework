namespace DragonSpark.Identity.PayPal;

public class PayPalClaim : Text.Text
{
	protected PayPalClaim(string name) : base($"{PayPalClaimNamespace.Default}:{name}") {}
}