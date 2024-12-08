namespace DragonSpark.Application.AspNet.Security.Identity.Model;

sealed class ProviderName : Text.Text
{
	public static ProviderName Default { get; } = new();

	ProviderName() : base("provider") {}
}