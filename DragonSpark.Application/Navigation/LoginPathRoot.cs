namespace DragonSpark.Application.Navigation;

public sealed class LoginPathRoot : Text.Text
{
	public static LoginPathRoot Default { get; } = new();

	LoginPathRoot() : base("/Identity/Account/Login") {}
}