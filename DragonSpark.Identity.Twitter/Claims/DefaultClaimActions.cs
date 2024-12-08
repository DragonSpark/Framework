using DragonSpark.Application.AspNet.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.Twitter.Claims;

public sealed class DefaultClaimActions : CompositeClaimAction
{
	public static DefaultClaimActions Default { get; } = new();

	DefaultClaimActions() : base(ImagePathClaimAction.Default, DescriptionClaimAction.Default,
	                             /**/
	                             LocationClaimAction.Default,
	                             WebsiteClaimAction.Default,
	                             FollowersClaimAction.Default,
	                             VerifiedClaimAction.Default) {}
}