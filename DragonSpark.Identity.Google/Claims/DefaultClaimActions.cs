using DragonSpark.Application.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.Google.Claims
{
	public sealed class DefaultClaimActions : CompositeClaimAction
	{
		public static DefaultClaimActions Default { get; } = new();

		DefaultClaimActions() : base(PictureClaimAction.Default) {}
	}
}