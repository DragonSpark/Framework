using DragonSpark.Application.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.PayPal;

public sealed class CountryClaimAction : SubKeyClaimAction
{
	public static CountryClaimAction Default { get; } = new();

	CountryClaimAction() : base(Country.Default, "address", "country") {}
}