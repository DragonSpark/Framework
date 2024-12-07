using DragonSpark.Application.Security.Identity.Claims.Compile;
using System.Collections.Generic;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Model;

sealed class KnownClaims : IKnownClaims
{
	public static KnownClaims Default { get; } = new();

	KnownClaims() {}

	public IEnumerable<string> Get()
	{
		yield return ClaimTypes.AuthenticationMethod;
		yield return ExternalIdentity.Default;
		yield return DisplayName.Default;
	}
}