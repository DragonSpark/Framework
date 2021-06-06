namespace DragonSpark.Identity.Twitter.Claims
{
	public sealed class Followers : TwitterClaim
	{
		public static Followers Default { get; } = new Followers();

		Followers() : base(nameof(Followers).ToLower()) {}
	}
}