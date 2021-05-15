using DragonSpark.Application.Compose;

namespace DragonSpark.Identity.Coinbase
{
	public static class Extensions
	{
		public static AuthenticationContext UsingCoinbase(this AuthenticationContext @this)
			=> @this.Then(ConfigureApplication.Default);
	}
}