using DragonSpark.Application.Compose;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using System;

namespace DragonSpark.Identity.Microsoft;

public static class Extensions
{
	public static AuthenticationContext UsingMicrosoft(this AuthenticationContext @this)
		=> UsingMicrosoft(@this, _ => {});

	public static AuthenticationContext UsingMicrosoft(this AuthenticationContext @this,
	                                                   Action<MicrosoftAccountOptions> configure)
		=> @this.Append(new ConfigureApplication(configure));
}