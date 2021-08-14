using DragonSpark.Application.Compose;
using DragonSpark.Application.Security.Identity.Claims.Actions;
using DragonSpark.Identity.Twitter.Claims;

namespace DragonSpark.Identity.Twitter
{
	public static class Extensions
	{
		public static AuthenticationContext UsingTwitter(this AuthenticationContext @this)
			=> @this.UsingTwitter(DefaultClaimActions.Default);

		public static AuthenticationContext UsingTwitter(this AuthenticationContext @this, IClaimAction claims)
			=> @this.Append(new ConfigureTwitterApplication(claims));

		public static ApplicationProfileContext WithTwitterApi(this ApplicationProfileContext @this)
			=> @this.Then(Registrations.Default);
	}
}