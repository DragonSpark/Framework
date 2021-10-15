namespace DragonSpark.Application.Security.Identity.Model;

public sealed class LoginErrorRedirect : ErrorRedirect
{
	public LoginErrorRedirect(string message, string origin) : base("./Login", message, origin) {}
}