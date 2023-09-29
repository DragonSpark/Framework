using DragonSpark.Application.Navigation;

namespace DragonSpark.Application.Security.Identity.Model;

public sealed class SignOutReturnPath : ReturnPath
{
	public static SignOutReturnPath Default { get; } = new();

	SignOutReturnPath() : base(SignOutPath.Default) {}
}