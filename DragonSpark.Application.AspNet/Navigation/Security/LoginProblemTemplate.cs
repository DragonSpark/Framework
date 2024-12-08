namespace DragonSpark.Application.AspNet.Navigation.Security;

public sealed class LoginProblemTemplate : Text.Text
{
	public static LoginProblemTemplate Default { get; } = new();

	LoginProblemTemplate() : base("/account/login/problem") {}
}