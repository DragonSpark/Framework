using DragonSpark.Compose;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace DragonSpark.Identity.Twitter
{
	public static class Extensions
	{
		public static Claim DisplayName(this ClaimsPrincipal @this) => @this.FindFirst(Twitter.DisplayName.Default);

		public static IServiceCollection WithTwitterAuthentication(this IServiceCollection @this)
			=> ConfigureTwitterApplication.Default.Parameter(@this);
	}
}