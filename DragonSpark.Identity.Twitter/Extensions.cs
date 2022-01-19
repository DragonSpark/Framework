using DragonSpark.Application.Compose;
using DragonSpark.Application.Security.Identity.Claims.Actions;
using DragonSpark.Identity.Twitter.Claims;
using Microsoft.AspNetCore.Authentication.Twitter;
using System;

namespace DragonSpark.Identity.Twitter;

public static class Extensions
{
	public static AuthenticationContext UsingTwitter(this AuthenticationContext @this) => UsingTwitter(@this, _ => {});

	public static AuthenticationContext UsingTwitter(this AuthenticationContext @this, Action<TwitterOptions> configure)
		=> @this.UsingTwitter(DefaultClaimActions.Default, configure);

	public static AuthenticationContext UsingTwitter(this AuthenticationContext @this, IClaimAction claims,
	                                                 Action<TwitterOptions> configure)
		=> @this.Append(new ConfigureTwitterApplication(claims, configure));

	public static ApplicationProfileContext WithTwitterApi(this ApplicationProfileContext @this)
		=> @this.Append(Registrations.Default);
}