using DragonSpark.Application.Compose;
using DragonSpark.Application.Security.Identity.Claims.Actions;
using DragonSpark.Compose;
using DragonSpark.Identity.Mixcloud.Api;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Identity.Mixcloud
{
	public static class Extensions
	{
		public static AuthenticationContext UsingMixcloud(this AuthenticationContext @this)
			=> @this.Append(ConfigureApplication.Default);

		public static AuthenticationContext UsingMixcloud(this AuthenticationContext @this, IClaimAction claims)
			=> @this.Append(new ConfigureApplication(claims));

		public static IServiceCollection WithMixcloudUserLookup(this IServiceCollection @this)
			=> Registrations.Default.Parameter(@this);
	}
}