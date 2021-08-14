using DragonSpark.Application.Compose;
using DragonSpark.Application.Security.Identity.Claims.Actions;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication.Facebook;
using System;

namespace DragonSpark.Identity.Facebook
{
	public static class Extensions
	{
		public static AuthenticationContext UsingFacebook(this AuthenticationContext @this, IClaimAction claims)
			=> @this.UsingFacebook(Start.A.Selection<FacebookOptions>()
			                            .By.Calling(x => x.ClaimActions)
			                            .Terminate(claims));

		public static AuthenticationContext UsingFacebook(this AuthenticationContext @this)
			=> @this.UsingFacebook(_ => {});

		public static AuthenticationContext UsingFacebook(this AuthenticationContext @this,
		                                                  ICommand<FacebookOptions> configure)
			=> @this.UsingFacebook(configure.Execute);

		public static AuthenticationContext UsingFacebook(this AuthenticationContext @this,
		                                                  Action<FacebookOptions> configure)
			=> @this.Append(new ConfigureApplication(configure));
	}
}