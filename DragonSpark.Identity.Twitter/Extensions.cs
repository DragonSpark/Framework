using DragonSpark.Application.Compose;
using System.Security.Claims;

namespace DragonSpark.Identity.Twitter
{
	public static class Extensions
	{
		public static Claim DisplayName(this ClaimsPrincipal @this) => @this.FindFirst(Twitter.DisplayName.Default);

		public static AuthenticationContext UsingTwitter(this AuthenticationContext @this)
			=> @this.Then(ConfigureTwitterApplication.Default);
	}
}