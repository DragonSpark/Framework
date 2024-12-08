using DragonSpark.Application.AspNet.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.Reddit.Claims;

public sealed class VerifiedClaimAction : ClaimAction
{
	public static VerifiedClaimAction Default { get; } = new();

	VerifiedClaimAction() : base(Verified.Default, Verified.Default) {}
}