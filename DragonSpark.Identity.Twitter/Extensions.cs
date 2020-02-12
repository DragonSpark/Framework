using DragonSpark.Application.Compose;
using System.Security.Claims;

namespace DragonSpark.Identity.Twitter
{
	public static class Extensions
	{
		public static Claim DisplayName(this ClaimsPrincipal @this) => @this.FindFirst(Twitter.DisplayName.Default);

		public static ApplicationProfileContext WithTwitterAuthentication(this ApplicationProfileContext @this)
			=> @this.Then(ConfigureTwitterApplication.Default);
	}
}