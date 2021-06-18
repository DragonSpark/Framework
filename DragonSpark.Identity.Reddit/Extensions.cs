using DragonSpark.Application.Compose;
using DragonSpark.Application.Security;

namespace DragonSpark.Identity.Reddit
{
	public static class Extensions
	{
		public static AuthenticationContext UsingReddit(this AuthenticationContext @this)
			=> @this.Append(ConfigureApplication.Default);

		public static AuthenticationContext UsingReddit(this AuthenticationContext @this, IClaimAction claims)
			=> @this.Append(new ConfigureApplication(claims));
	}
}