namespace DragonSpark.Identity.Reddit.Claims
{
	public sealed class Description : RedditClaim
	{
		public static Description Default { get; } = new Description();

		Description() : base(nameof(Description).ToLowerInvariant()) {}
	}
}