using DragonSpark.Application.AspNet.Security.Identity.Claims.Access;

namespace DragonSpark.Application.AspNet.Security.Identity;

public sealed class ExternalIdentityValue : RequiredClaim
{
	public static ExternalIdentityValue Default { get; } = new();

	ExternalIdentityValue() : base(ExternalIdentity.Default) {}
}