namespace DragonSpark.Identity.Mixcloud.Claims
{
	public sealed class IsPremiumAccount : MixcloudClaim
	{
		public static IsPremiumAccount Default { get; } = new();

		IsPremiumAccount() : base(nameof(IsPremiumAccount).ToLower()) {}
	}
}