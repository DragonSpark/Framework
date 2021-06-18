namespace DragonSpark.Identity.Facebook.Claims
{
	public sealed class DisplayName : FacebookClaim
	{
		public static DisplayName Default { get; } = new DisplayName();

		DisplayName() : base(nameof(DisplayName).ToLower()) {}
	}
}