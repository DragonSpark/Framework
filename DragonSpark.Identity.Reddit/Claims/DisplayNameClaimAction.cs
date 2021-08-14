using DragonSpark.Application.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.Reddit.Claims
{
	public sealed class DisplayNameClaimAction : SubKeyClaimAction
	{
		public static DisplayNameClaimAction Default { get; } = new DisplayNameClaimAction();

		DisplayNameClaimAction() : base(DisplayName.Default, "subreddit", "title") {}
	}
}