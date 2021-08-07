using DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Identity.Patreon.Claims
{
	public sealed class AboutClaimAction : ClaimAction
	{
		public static AboutClaimAction Default { get; } = new AboutClaimAction();

		AboutClaimAction() : base(About.Default, "about") {}
	}
}
