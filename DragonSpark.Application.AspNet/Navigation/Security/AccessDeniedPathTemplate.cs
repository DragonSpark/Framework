namespace DragonSpark.Application.AspNet.Navigation.Security;

public sealed class AccessDeniedPathTemplate : Text.Text
{
	public static AccessDeniedPathTemplate Default { get; } = new();

	AccessDeniedPathTemplate() : base("Identity/Account/AccessDenied?ReturnUrl={0}") {}
}