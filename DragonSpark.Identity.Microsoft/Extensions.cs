using DragonSpark.Application.Compose;

namespace DragonSpark.Identity.Microsoft
{
	public static class Extensions
	{
		public static AuthenticationContext UsingMicrosoft(this AuthenticationContext @this)
			=> @this.Then(ConfigureApplication.Default);
	}
}