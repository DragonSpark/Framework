namespace DragonSpark.Identity.Twitter
{
	public sealed class DisplayName : TwitterClaim
	{
		public static DisplayName Default { get; } = new DisplayName();

		DisplayName() : base(nameof(DisplayName).ToLower()) {}
	}

	public sealed class ScreenName : TwitterClaim
	{
		public static ScreenName Default { get; } = new ScreenName();

		ScreenName() : base(nameof(ScreenName).ToLower()) {}
	}
}