namespace DragonSpark.Identity.Facebook.Claims
{
	public sealed class ScreenName : FacebookClaim
	{
		public static ScreenName Default { get; } = new ScreenName();

		ScreenName() : base(nameof(ScreenName).ToLower()) {}
	}
}