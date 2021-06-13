using DragonSpark.Application.Security.Identity;

namespace DragonSpark.Application.Security
{
	public sealed class ExternalIdentityValue : RequiredClaim
	{
		public static ExternalIdentityValue Default { get; } = new ExternalIdentityValue();

		ExternalIdentityValue() : base(ExternalIdentity.Default) {}
	}
}