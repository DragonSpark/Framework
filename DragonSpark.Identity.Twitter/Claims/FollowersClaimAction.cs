using DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Identity.Twitter.Claims
{
	public sealed class FollowersClaimAction : ClaimAction
	{
		public static FollowersClaimAction Default { get; } = new FollowersClaimAction();

		FollowersClaimAction() : base(Followers.Default, "followers_count", "integer") {}
	}
}