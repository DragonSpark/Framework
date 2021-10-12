using DragonSpark.Application.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.Mixcloud.Claims
{
	public sealed class DefaultClaimActions : CompositeClaimAction
	{
		public static DefaultClaimActions Default { get; } = new();

		DefaultClaimActions() : base(IsProfessionalAccountAction.Default, FollowersAction.Default,
		                             CloudCastsAction.Default) {}
	}
}