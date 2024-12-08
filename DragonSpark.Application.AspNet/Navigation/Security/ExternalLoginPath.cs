namespace DragonSpark.Application.AspNet.Navigation.Security;

public sealed class ExternalLoginPath : DragonSpark.Text.Text
{
	public static ExternalLoginPath Default { get; } = new ();

	ExternalLoginPath() : base("/Identity/Account/ExternalLogin") {}
}