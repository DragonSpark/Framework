namespace DragonSpark.Identity.Mixcloud.Claims;

public sealed class CloudCasts : MixcloudClaim
{
	public static CloudCasts Default { get; } = new();

	CloudCasts() : base(nameof(CloudCasts).ToLower()) {}
}