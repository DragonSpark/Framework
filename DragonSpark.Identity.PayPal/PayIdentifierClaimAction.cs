using DragonSpark.Application.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.PayPal;

public sealed class PayIdentifierClaimAction : ClaimAction
{
	public static PayIdentifierClaimAction Default { get; } = new();

	PayIdentifierClaimAction() : base(PayIdentifier.Default, "payer_id") {}
}