using DragonSpark.Application.Security;

namespace DragonSpark.Identity.Twitter.Claims {
	public sealed class VerifiedClaimAction : ClaimAction
	{
		public static VerifiedClaimAction Default { get; } = new VerifiedClaimAction();

		VerifiedClaimAction() : base(Verified.Default, "verified", "boolean") {}
	}
}