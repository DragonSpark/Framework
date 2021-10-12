using DragonSpark.Application.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.Mixcloud.Claims
{
	public class FollowersAction : ClaimAction
	{
		public static FollowersAction Default { get; } = new();

		FollowersAction() : base(Followers.Default, "follower_count", "integer") {}
	}
}