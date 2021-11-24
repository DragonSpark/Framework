namespace DragonSpark.Identity.Mixcloud.Claims;

public sealed class IsProfessionalAccount : MixcloudClaim
{
	public static IsProfessionalAccount Default { get; } = new();

	IsProfessionalAccount() : base(nameof(IsProfessionalAccount).ToLower()) {}
}