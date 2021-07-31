namespace DragonSpark.Identity.Reddit.Claims
{
	public sealed class Verified : RedditClaim
	{
		public static Verified Default { get; } = new Verified();

		Verified() : base(nameof(Verified).ToLowerInvariant()) {}
	}
}