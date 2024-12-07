namespace DragonSpark.Application.Navigation.Security;

public sealed class LoginForbiddenPath : Text.Text
{
	public static LoginForbiddenPath Default { get; } = new();

	LoginForbiddenPath() : base("/account/login/forbidden") {}
}