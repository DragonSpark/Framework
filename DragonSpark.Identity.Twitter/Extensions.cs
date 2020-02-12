using DragonSpark.Application.Compose;
using System.Security.Claims;

namespace DragonSpark.Identity.Twitter
{
	public static class Extensions
	{
		public static Claim DisplayName(this ClaimsPrincipal @this) => @this.FindFirst(Twitter.DisplayName.Default);

		public static ServerProfileContext WithTwitterAuthentication(this ServerProfileContext @this)
			=> @this.Then(ConfigureTwitterApplication.Default);
	}
}