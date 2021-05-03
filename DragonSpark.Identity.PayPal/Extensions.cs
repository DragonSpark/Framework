using DragonSpark.Application.Compose;

namespace DragonSpark.Identity.PayPal
{
	public static class Extensions
	{
		public static AuthenticationContext UsingPayPal(this AuthenticationContext @this)
			=> @this.Then(ConfigureApplication.Default);
	}
}