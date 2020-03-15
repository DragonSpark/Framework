namespace DragonSpark.Identity.Twitter {
	public sealed class ScreenName : TwitterClaim
	{
		public static ScreenName Default { get; } = new ScreenName();

		ScreenName() : base(nameof(ScreenName).ToLower()) {}
	}
}