namespace DragonSpark.Identity.Reddit.Claims
{
	public sealed class ScreenName : RedditClaim
	{
		public static ScreenName Default { get; } = new ScreenName();

		ScreenName() : base(nameof(ScreenName).ToLower()) {}
	}
}