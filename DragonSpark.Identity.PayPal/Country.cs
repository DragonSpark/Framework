namespace DragonSpark.Identity.PayPal;

public sealed class Country : PayPalClaim
{
	public static Country Default { get; } = new();

	Country() : base(nameof(Country).ToLower()) {}
}