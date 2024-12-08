using DragonSpark.Application.AspNet.Compose;
using Microsoft.AspNetCore.Authentication.Google;
using System;

namespace DragonSpark.Identity.Google;

public static class Extensions
{
	public static AuthenticationContext UsingGoogle(this AuthenticationContext @this) => UsingGoogle(@this, _ => {});

	public static AuthenticationContext UsingGoogle(this AuthenticationContext @this, Action<GoogleOptions> configure)
		=> @this.Append(new ConfigureApplication(configure));
}