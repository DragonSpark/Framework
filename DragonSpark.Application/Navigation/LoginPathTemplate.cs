namespace DragonSpark.Application.Navigation;

public sealed class LoginPathTemplate : Text.Text
{
	public static LoginPathTemplate Default { get; } = new LoginPathTemplate();

	LoginPathTemplate() : base("Identity/Account/Login?ReturnUrl={0}") {}
}