namespace DragonSpark.Application.AspNet.Navigation.Security;

public sealed class LoginPathTemplate : Text.Text
{
	public static LoginPathTemplate Default { get; } = new();

	LoginPathTemplate() : base($"{LoginPathRoot.Default}?ReturnUrl={{0}}") {}
}