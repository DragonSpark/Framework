using DragonSpark.Application.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.DeviantArt.Claims
{
	public sealed class WebsiteClaimAction : SubKeyClaimAction
	{
		public static WebsiteClaimAction Default { get; } = new ();

		WebsiteClaimAction() : base(Website.Default, "profile", "website") {}
	}
}