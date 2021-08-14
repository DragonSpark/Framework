using AspNet.Security.OAuth.Patreon;
using DragonSpark.Application.Compose;
using DragonSpark.Application.Security.Identity.Claims.Actions;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Identity.Patreon
{
	public static class Extensions
	{
		public static AuthenticationContext UsingPatreon(this AuthenticationContext @this, IClaimAction claims)
			=> @this.UsingPatreon(Start.A.Selection<PatreonAuthenticationOptions>()
			                            .By.Calling(x => x.ClaimActions)
			                            .Terminate(claims));

		public static AuthenticationContext UsingPatreon(this AuthenticationContext @this)
			=> @this.UsingPatreon(_ => {});

		public static AuthenticationContext UsingPatreon(this AuthenticationContext @this,
		                                                  ICommand<PatreonAuthenticationOptions> configure)
			=> @this.UsingPatreon(configure.Execute);

		public static AuthenticationContext UsingPatreon(this AuthenticationContext @this,
		                                                  Action<PatreonAuthenticationOptions> configure)
			=> @this.Append(new ConfigureApplication(configure));
	}
}