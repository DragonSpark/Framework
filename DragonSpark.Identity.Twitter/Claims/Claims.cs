using DragonSpark.Application.Security.Identity;

namespace DragonSpark.Identity.Twitter.Claims
{
	public sealed class Claims : Application.Security.Identity.Claims
	{
		public static Claims Default { get; } = new Claims();

		Claims() : base(x => x.Type.StartsWith(ClaimNamespace.Default)
		                     ||
		                     x.Type.StartsWith(TwitterClaimNamespace.Default)) {}
	}
}