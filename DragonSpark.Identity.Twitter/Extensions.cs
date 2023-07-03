using AspNet.Security.OAuth.Twitter;
using DragonSpark.Application.Compose;
using DragonSpark.Application.Security.Identity.Claims.Actions;
using DragonSpark.Identity.Twitter.Claims;
using System;

namespace DragonSpark.Identity.Twitter;

public static class Extensions
{
	public static AuthenticationContext UsingTwitter(this AuthenticationContext @this)
		=> UsingTwitter(@this, DefaultTwitterConfiguration.Default.Execute);

	public static AuthenticationContext UsingTwitter(this AuthenticationContext @this,
	                                                 Action<TwitterAuthenticationOptions> configure)
		=> @this.UsingTwitter(DefaultClaimActions.Default, configure);

	public static AuthenticationContext UsingTwitter(this AuthenticationContext @this, IClaimAction claims,
	                                                 Action<TwitterAuthenticationOptions> configure)
		=> @this.Append(new ConfigureTwitterApplication(claims, configure));

	public static ApplicationProfileContext WithTwitterApi(this ApplicationProfileContext @this)
		=> @this.Append(Registrations.Default);
}