using DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity;

public sealed class ExternalIdentity : Text.Text
{
	public static ExternalIdentity Default { get; } = new ();

	ExternalIdentity() : base($"{ClaimNamespace.Default}:identity") {}
}