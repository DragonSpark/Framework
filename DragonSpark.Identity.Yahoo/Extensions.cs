using AspNet.Security.OAuth.Yahoo;
using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Application.AspNet.Security.Identity.Claims.Actions;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Identity.Yahoo;

public static class Extensions
{
	public static AuthenticationContext UsingYahoo(this AuthenticationContext @this, IClaimAction claims)
		=> @this.UsingYahoo(Start.A.Selection<YahooAuthenticationOptions>()
		                         .By.Calling(x => x.ClaimActions)
		                         .Terminate(claims));

	public static AuthenticationContext UsingYahoo(this AuthenticationContext @this) => @this.UsingYahoo(_ => {});

	public static AuthenticationContext UsingYahoo(this AuthenticationContext @this,
	                                               ICommand<YahooAuthenticationOptions> configure)
		=> @this.UsingYahoo(configure.Execute);

	public static AuthenticationContext UsingYahoo(this AuthenticationContext @this,
	                                               Action<YahooAuthenticationOptions> configure)
		=> @this.Append(new ConfigureApplication(configure));
}