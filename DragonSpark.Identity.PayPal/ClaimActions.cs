using DragonSpark.Application.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.PayPal;

public sealed class ClaimActions : CompositeClaimAction
{
	public static ClaimActions Default { get; } = new();

	ClaimActions() : base(PayIdentifierClaimAction.Default, CountryClaimAction.Default) {}
}