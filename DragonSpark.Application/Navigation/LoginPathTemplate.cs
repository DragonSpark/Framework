namespace DragonSpark.Application.Navigation;

public sealed class LoginPathTemplate : Text.Text
{
	public static LoginPathTemplate Default { get; } = new LoginPathTemplate();

	LoginPathTemplate() : base($"{LoginPathRoot.Default}?ReturnUrl={{0}}") {}
}