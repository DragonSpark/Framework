using AspNet.Security.OAuth.Coinbase;
using DragonSpark.Application.Security.Identity.Authentication;

namespace DragonSpark.Identity.Coinbase
{
	public class SetCoinbaseScopes : SetScopes<CoinbaseAuthenticationOptions>
	{
		protected SetCoinbaseScopes(params string[] scopes) : base(scopes) {}
	}
}