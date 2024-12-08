using AspNet.Security.OAuth.DeviantArt;
using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Application.AspNet.Security.Identity.Claims.Actions;
using DragonSpark.Compose;
using DragonSpark.Identity.DeviantArt.Api;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Identity.DeviantArt;

public static class Extensions
{
	public static AuthenticationContext UsingDeviantArt(this AuthenticationContext @this)
		=> UsingDeviantArt(@this, _ => {});

	public static AuthenticationContext UsingDeviantArt(this AuthenticationContext @this,
	                                                    Action<DeviantArtAuthenticationOptions> configure)
		=> @this.Append(new ConfigureApplication(configure));

	public static AuthenticationContext UsingDeviantArt(this AuthenticationContext @this, IClaimAction claims)
		=> UsingDeviantArt(@this, claims, _ => {});

	public static AuthenticationContext UsingDeviantArt(this AuthenticationContext @this, IClaimAction claims,
	                                                    Action<DeviantArtAuthenticationOptions> configure)
		=> @this.Append(new ConfigureApplication(claims, configure));

	public static IServiceCollection WithDeviantArtUserLookup(this IServiceCollection @this)
		=> Registrations.Default.Parameter(@this);
}