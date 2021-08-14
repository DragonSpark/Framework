using DragonSpark.Application.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.DeviantArt.Claims
{
	public sealed class DefaultClaimActions : CompositeClaimAction
	{
		public static DefaultClaimActions Default { get; } = new();

		DefaultClaimActions() : base(DisplayNameClaimAction.Default, ImageClaimAction.Default,
		                             DescriptionClaimAction.Default) {}
	}
}