using DragonSpark.Application.Security.Identity.Claims.Access;

namespace DragonSpark.Application.Security.Identity
{
	public sealed class ExternalIdentity : Text.Text
	{
		public static ExternalIdentity Default { get; } = new ();

		ExternalIdentity() : base($"{ClaimNamespace.Default}:identity") {}
	}
}