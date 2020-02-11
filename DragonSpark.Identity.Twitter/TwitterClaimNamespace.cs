using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Identity.Twitter
{
	public sealed class TwitterClaimNamespace : Text.Text
	{
		public static TwitterClaimNamespace Default { get; } = new TwitterClaimNamespace();

		TwitterClaimNamespace() : base("urn:twitter") {}
	}

	public sealed class Claims : Application.Security.Claims
	{
		public static Claims Default { get; } = new Claims();

		Claims() : base(x => x.Type.StartsWith(TwitterClaimNamespace.Default)) {}
	}

	sealed class UserSynchronizer<T> : Application.Security.UserSynchronizer<T> where T : IdentityUser
	{
		public UserSynchronizer(UserManager<T> users) : base(users, Claims.Default) {}
	}
}