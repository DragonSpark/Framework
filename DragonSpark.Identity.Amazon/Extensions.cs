using AspNet.Security.OAuth.Amazon;
using DragonSpark.Application.Compose;
using DragonSpark.Application.Security.Identity.Claims.Actions;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Identity.Amazon
{
	public static class Extensions
	{
		public static AuthenticationContext UsingAmazon(this AuthenticationContext @this, IClaimAction claims)
			=> @this.UsingAmazon(Start.A.Selection<AmazonAuthenticationOptions>()
			                          .By.Calling(x => x.ClaimActions)
			                          .Terminate(claims));

		public static AuthenticationContext UsingAmazon(this AuthenticationContext @this)
			=> @this.UsingAmazon(_ => {});

		public static AuthenticationContext UsingAmazon(this AuthenticationContext @this,
		                                                ICommand<AmazonAuthenticationOptions> configure)
			=> @this.UsingAmazon(configure.Execute);

		public static AuthenticationContext UsingAmazon(this AuthenticationContext @this,
		                                                Action<AmazonAuthenticationOptions> configure)
			=> @this.Append(new ConfigureApplication(configure));
	}
}