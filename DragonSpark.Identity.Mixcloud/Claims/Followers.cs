namespace DragonSpark.Identity.Mixcloud.Claims;

public sealed class Followers : MixcloudClaim
{
	public static Followers Default { get; } = new();

	Followers() : base(nameof(Followers).ToLower()) {}
}