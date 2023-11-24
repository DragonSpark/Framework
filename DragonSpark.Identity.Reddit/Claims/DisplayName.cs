namespace DragonSpark.Identity.Reddit.Claims;

public sealed class DisplayName : RedditClaim
{
	public static DisplayName Default { get; } = new();

	DisplayName() : base(nameof(DisplayName).ToLower()) {}
}