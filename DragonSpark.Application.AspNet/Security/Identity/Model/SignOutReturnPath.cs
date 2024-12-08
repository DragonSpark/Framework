using DragonSpark.Application.AspNet.Navigation;

namespace DragonSpark.Application.AspNet.Security.Identity.Model;

public sealed class SignOutReturnPath : ReturnPath
{
	public static SignOutReturnPath Default { get; } = new();

	SignOutReturnPath() : base(SignOutPath.Default) {}
}