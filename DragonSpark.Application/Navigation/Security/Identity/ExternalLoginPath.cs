namespace DragonSpark.Application.Navigation.Security.Identity;

public sealed class ExternalLoginPath : DragonSpark.Text.Text
{
	public static ExternalLoginPath Default { get; } = new ExternalLoginPath();

	ExternalLoginPath() : base("/Identity/Account/ExternalLogin") {}
}