using AspNet.Security.OAuth.Coinbase;
using DragonSpark.Application.Compose;
using DragonSpark.Application.Security.Identity.Claims.Actions;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Identity.Coinbase
{
	public static class Extensions
	{
		public static AuthenticationContext UsingCoinbase(this AuthenticationContext @this, IClaimAction claims)
			=> @this.UsingCoinbase(Start.A.Selection<CoinbaseAuthenticationOptions>()
			                            .By.Calling(x => x.ClaimActions)
			                            .Terminate(claims));

		public static AuthenticationContext UsingCoinbase(this AuthenticationContext @this)
			=> @this.UsingCoinbase(_ => {});

		public static AuthenticationContext UsingCoinbase(this AuthenticationContext @this,
		                                                  ICommand<CoinbaseAuthenticationOptions> configure)
			=> @this.UsingCoinbase(configure.Execute);

		public static AuthenticationContext UsingCoinbase(this AuthenticationContext @this,
		                                                  Action<CoinbaseAuthenticationOptions> configure)
			=> @this.Append(new ConfigureApplication(configure));
	}
}