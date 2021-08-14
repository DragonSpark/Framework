using DragonSpark.Application.Security.Identity.Claims.Actions;
using Microsoft.AspNetCore.Authentication;

namespace DragonSpark.Identity.Twitter.Claims
{
	public sealed class WebsiteClaimAction : CustomClaimAction
	{
		public static WebsiteClaimAction Default { get; } = new WebsiteClaimAction();

		WebsiteClaimAction() : base(Website.Default, "url", root => root.GetProperty("entities")
		                                                                .GetProperty("url")
		                                                                .GetProperty("urls")[0]
		                                                                .GetString("expanded_url")!) {}
	}
}