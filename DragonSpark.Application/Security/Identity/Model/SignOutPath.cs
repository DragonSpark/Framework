namespace DragonSpark.Application.Security.Identity.Model;

public sealed class SignOutPath : Text.Text
{
	public static SignOutPath Default { get; } = new();

	SignOutPath() : base("/Identity/Account/LogOut") {}
}