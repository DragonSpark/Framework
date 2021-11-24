namespace DragonSpark.Identity.Twitter.Claims;

public sealed class DisplayName : TwitterClaim
{
	public static DisplayName Default { get; } = new DisplayName();

	DisplayName() : base(nameof(DisplayName).ToLower()) {}
}