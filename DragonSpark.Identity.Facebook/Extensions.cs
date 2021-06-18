using DragonSpark.Application.Compose;
using DragonSpark.Application.Security;

namespace DragonSpark.Identity.Facebook
{
	public static class Extensions
	{
		public static AuthenticationContext UsingFacebook(this AuthenticationContext @this)
			=> @this.Append(ConfigureApplication.Default);

		public static AuthenticationContext UsingFacebook(this AuthenticationContext @this, IClaimAction claims)
			=> @this.Append(new ConfigureApplication(claims));
	}
}