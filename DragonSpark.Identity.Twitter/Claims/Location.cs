namespace DragonSpark.Identity.Twitter.Claims;

public sealed class Location : TwitterClaim
{
	public static Location Default { get; } = new();

	Location() : base(nameof(Location).ToLower()) {}
}