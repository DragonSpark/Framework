namespace DragonSpark.Application.Navigation;

public sealed class LoginProblemPath : Text.Text
{
	public static LoginProblemPath Default { get; } = new();

	LoginProblemPath() : base("/account/login/problem") {}
}