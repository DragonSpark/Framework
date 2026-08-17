namespace DragonSpark.Application.Security.Identity.Claims;

public sealed class ClaimNamespace : Text.Text
{
	public static ClaimNamespace Default { get; } = new();

	ClaimNamespace() : base("urn:dragonspark") {}
}

public class Claim : Text.Text // TODO
{
	protected Claim(string name) : base($"{ClaimNamespace.Default}:{name}") {}
}