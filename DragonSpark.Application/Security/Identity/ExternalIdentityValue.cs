using DragonSpark.Application.Security.Identity.Claims.Access;

namespace DragonSpark.Application.Security.Identity;

public sealed class ExternalIdentityValue : RequiredClaim
{
	public static ExternalIdentityValue Default { get; } = new();

	ExternalIdentityValue() : base(ExternalIdentity.Default) {}
}