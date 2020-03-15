using DragonSpark.Application.Security.Identity.Profile;

namespace DragonSpark.Identity.Twitter
{
	public sealed class Claims : Application.Security.Identity.Claims
	{
		public static Claims Default { get; } = new Claims();

		Claims() : base(x => x.Type.StartsWith(ClaimNamespace.Default)
		                     ||
		                     x.Type.StartsWith(TwitterClaimNamespace.Default)) {}
	}
}