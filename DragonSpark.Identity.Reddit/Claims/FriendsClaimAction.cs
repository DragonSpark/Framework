using DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Identity.Reddit.Claims
{
	public sealed class FriendsClaimAction : ClaimAction
	{
		public static FriendsClaimAction Default { get; } = new FriendsClaimAction();

		FriendsClaimAction() : base(Friends.Default, "num_friends", "integer") {}
	}
}