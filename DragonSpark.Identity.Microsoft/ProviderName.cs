namespace DragonSpark.Identity.Microsoft;

public sealed class ProviderName : Text.Text
{
	public static ProviderName Default { get; } = new();

	ProviderName() : base("Microsoft") {}
}