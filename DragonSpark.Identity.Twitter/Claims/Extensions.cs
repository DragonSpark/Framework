using DragonSpark.Application.Compose;

namespace DragonSpark.Identity.Twitter.Claims
{
	public static class Extensions
	{
		public static AuthenticationContext UsingTwitter(this AuthenticationContext @this)
			=> @this.Then(ConfigureTwitterApplication.Default);
	}
}