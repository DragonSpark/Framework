using DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Application.Security.Identity
{
	public sealed class ExternalIdentityValue : RequiredClaim
	{
		public static ExternalIdentityValue Default { get; } = new ExternalIdentityValue();

		ExternalIdentityValue() : base(ExternalIdentity.Default) {}
	}
}