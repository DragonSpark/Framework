using DragonSpark.Application.AspNet.Security.Identity.Claims.Actions;
using JetBrains.Annotations;

namespace DragonSpark.Identity.DeviantArt.Claims;

public sealed class WebsiteClaimAction : SubKeyClaimAction
{
	[UsedImplicitly]
	public static WebsiteClaimAction Default { get; } = new ();

	WebsiteClaimAction() : base(Website.Default, "profile", "website") {}
}