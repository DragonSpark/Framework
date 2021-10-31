namespace DragonSpark.Application.Security.Identity.MultiFactor;

sealed class KeyApplicationLocationTemplate : Text.Text
{
	public static KeyApplicationLocationTemplate Default { get; } = new();

	KeyApplicationLocationTemplate() : base("otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6") {}
}