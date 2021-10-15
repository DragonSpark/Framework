namespace DragonSpark.Application.Navigation;

public sealed class AccessDeniedPathTemplate : Text.Text
{
	public static AccessDeniedPathTemplate Default { get; } = new AccessDeniedPathTemplate();

	AccessDeniedPathTemplate() : base("Identity/Account/AccessDenied?ReturnUrl={0}") {}
}