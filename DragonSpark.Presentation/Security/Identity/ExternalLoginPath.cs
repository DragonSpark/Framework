namespace DragonSpark.Presentation.Security.Identity
{
	public sealed class ExternalLoginPath : Text.Text
	{
		public static ExternalLoginPath Default { get; } = new ExternalLoginPath();

		ExternalLoginPath() : base("/Identity/Account/ExternalLogin") {}
	}
}