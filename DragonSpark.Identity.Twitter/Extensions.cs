﻿using DragonSpark.Application.Compose;
using DragonSpark.Application.Security;
using DragonSpark.Identity.Twitter.Claims;

namespace DragonSpark.Identity.Twitter
{
	public static class Extensions
	{
		public static AuthenticationContext UsingTwitter(this AuthenticationContext @this)
			=> @this.UsingTwitter(DefaultClaimActions.Default);

		public static AuthenticationContext UsingTwitter(this AuthenticationContext @this, IClaimAction claims)
			=> @this.Then(new ConfigureTwitterApplication(claims));

		public static ApplicationProfileContext WithTwitterApi(this ApplicationProfileContext @this)
			=> @this.Then(Registrations.Default);
	}
}