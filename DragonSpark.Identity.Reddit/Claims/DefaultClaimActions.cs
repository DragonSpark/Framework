using DragonSpark.Application.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.Reddit.Claims
{
	public sealed class DefaultClaimActions : CompositeClaimAction
	{
		public static DefaultClaimActions Default { get; } = new DefaultClaimActions();

		DefaultClaimActions() : base(DisplayNameClaimAction.Default,
		                             ImageClaimAction.Default, DescriptionClaimAction.Default,
		                             VerifiedClaimAction.Default, FriendsClaimAction.Default) {}
	}
}