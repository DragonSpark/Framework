using AspNet.Security.OAuth.Discord;
using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Application.AspNet.Security.Identity.Claims.Actions;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Identity.Discord;

public static class Extensions
{
	public static AuthenticationContext UsingDiscord(this AuthenticationContext @this, IClaimAction claims)
		=> @this.UsingDiscord(Start.A.Selection<DiscordAuthenticationOptions>()
		                           .By.Calling(x => x.ClaimActions)
		                           .Terminate(claims));

	public static AuthenticationContext UsingDiscord(this AuthenticationContext @this) => @this.UsingDiscord(_ => {});

	public static AuthenticationContext UsingDiscord(this AuthenticationContext @this,
	                                                 ICommand<DiscordAuthenticationOptions> configure)
		=> @this.UsingDiscord(configure.Execute);

	public static AuthenticationContext UsingDiscord(this AuthenticationContext @this,
	                                                 Action<DiscordAuthenticationOptions> configure)
		=> @this.Append(new ConfigureApplication(configure));
}