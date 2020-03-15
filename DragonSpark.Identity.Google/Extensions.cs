using DragonSpark.Application.Compose;

namespace DragonSpark.Identity.Google
{
	public static class Extensions
	{
		public static AuthenticationContext UsingGoogle(this AuthenticationContext @this)
			=> @this.Then(ConfigureApplication.Default);
	}
}