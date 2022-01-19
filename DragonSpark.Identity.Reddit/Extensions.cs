using AspNet.Security.OAuth.Reddit;
using DragonSpark.Application.Compose;
using DragonSpark.Application.Security.Identity.Claims.Actions;
using System;

namespace DragonSpark.Identity.Reddit;

public static class Extensions
{
	public static AuthenticationContext UsingReddit(this AuthenticationContext @this)
		=> @this.Append(ConfigureApplication.Default);

	public static AuthenticationContext UsingReddit(this AuthenticationContext @this,
	                                                Action<RedditAuthenticationOptions> configure)
		=> @this.Append(new ConfigureApplication(configure));

	public static AuthenticationContext UsingReddit(this AuthenticationContext @this, IClaimAction claims,
	                                                Action<RedditAuthenticationOptions> configure)
		=> @this.Append(new ConfigureApplication(claims, configure));
}