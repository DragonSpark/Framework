using DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Identity.Facebook.Claims
{
	public sealed class DefaultClaimActions : CompositeClaimAction
	{
		public static DefaultClaimActions Default { get; } = new DefaultClaimActions();

		DefaultClaimActions() : base(DisplayNameClaimAction.Default) {}
	}
}