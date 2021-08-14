using DragonSpark.Application.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.Reddit.Claims
{
	public sealed class VerifiedClaimAction : ClaimAction
	{
		public static VerifiedClaimAction Default { get; } = new VerifiedClaimAction();

		VerifiedClaimAction() : base(Verified.Default, Verified.Default) {}
	}
}