namespace DragonSpark.Identity.Twitter.Claims;

public sealed class Followers : TwitterClaim
{
	public static Followers Default { get; } = new();

	Followers() : base(nameof(Followers).ToLower()) {}
}