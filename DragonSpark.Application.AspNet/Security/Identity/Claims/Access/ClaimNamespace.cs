namespace DragonSpark.Application.AspNet.Security.Identity.Claims.Access;

public sealed class ClaimNamespace : Text.Text
{
	public static ClaimNamespace Default { get; } = new();

	ClaimNamespace() : base("urn:dragonspark") {}
}