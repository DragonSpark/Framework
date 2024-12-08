namespace DragonSpark.Application.AspNet.Navigation.Security;

public sealed class LoginDeniedPath : Text.Text
{
	public static LoginDeniedPath Default { get; } = new();

	LoginDeniedPath() : base("/account/login/denied") {}
}