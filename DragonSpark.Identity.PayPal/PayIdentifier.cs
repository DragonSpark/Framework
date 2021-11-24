namespace DragonSpark.Identity.PayPal;

public sealed class PayIdentifier : PayPalClaim
{
	public static PayIdentifier Default { get; } = new PayIdentifier();

	PayIdentifier() : base(nameof(PayIdentifier).ToLower()) {}
}