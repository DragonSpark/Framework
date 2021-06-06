namespace DragonSpark.Identity.Twitter.Claims
{
	public sealed class Description : TwitterClaim
	{
		public static Description Default { get; } = new Description();

		Description() : base(nameof(Description).ToLower()) {}
	}
}