using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Passwordless;

public readonly record struct VerifyCodeInput<T>(UserManager<T> Session, T User, string Purpose, string Code) where T : class
{
	public VerifyCodeInput(UserManager<T> Session, T User, string Code)
		: this(Session, User, Passwordless.Purpose.Default, Code) {}
}