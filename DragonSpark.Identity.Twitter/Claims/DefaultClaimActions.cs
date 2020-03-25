using DragonSpark.Application.Security;

namespace DragonSpark.Identity.Twitter.Claims {
	public sealed class DefaultClaimActions : CompositeClaimAction
	{
		public static DefaultClaimActions Default { get; } = new DefaultClaimActions();

		DefaultClaimActions()
			: base(DisplayNameClaimAction.Default, ImagePathClaimAction.Default, DescriptionClaimAction.Default) {}
	}
}