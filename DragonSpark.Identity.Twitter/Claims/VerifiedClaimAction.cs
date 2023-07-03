using DragonSpark.Application.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.Twitter.Claims;

public sealed class VerifiedClaimAction : SubKeyClaimAction
{
	public static VerifiedClaimAction Default { get; } = new();

	VerifiedClaimAction() : base(Verified.Default, new SubKey("data", "verified"), "boolean") {}
}