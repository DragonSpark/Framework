namespace DragonSpark.Identity.Twitter.Claims;

public sealed class Description : TwitterClaim
{
	public static Description Default { get; } = new();

	Description() : base(nameof(Description).ToLower()) {}
}