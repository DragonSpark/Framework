using DragonSpark.Application.Compose;
using DragonSpark.Application.Security.Identity.Claims.Actions;
using DragonSpark.Compose;
using DragonSpark.Identity.DeviantArt.Api;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Identity.DeviantArt;

public static class Extensions
{
	public static AuthenticationContext UsingDeviantArt(this AuthenticationContext @this)
		=> @this.Append(ConfigureApplication.Default);

	public static AuthenticationContext UsingDeviantArt(this AuthenticationContext @this, IClaimAction claims)
		=> @this.Append(new ConfigureApplication(claims));

	public static IServiceCollection WithDeviantArtUserLookup(this IServiceCollection @this)
		=> Registrations.Default.Parameter(@this);
}