namespace DragonSpark.Identity.Twitter.Claims;

public sealed class Verified : TwitterClaim
{
	public static Verified Default { get; } = new();

	Verified() : base(nameof(Verified).ToLower()) {}
}