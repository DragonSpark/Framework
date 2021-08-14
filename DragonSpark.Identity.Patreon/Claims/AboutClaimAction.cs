using DragonSpark.Application.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.Patreon.Claims
{
	public sealed class AboutClaimAction : SubKeyClaimAction
	{
		public static AboutClaimAction Default { get; } = new AboutClaimAction();

		AboutClaimAction() : base(About.Default, "attributes", "about") {}
	}
}
