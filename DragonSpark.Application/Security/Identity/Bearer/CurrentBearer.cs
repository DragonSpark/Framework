using NetFabric.Hyperlinq;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class CurrentBearer : ICurrentBearer
{
	readonly ISign                   _sign;
	readonly ICurrentPrincipal       _principal;
	readonly DetermineBearerIdentity _identity;

	public CurrentBearer(ISign sign, ICurrentPrincipal principal, DetermineBearerIdentity identity)
	{
		_sign      = sign;
		_principal = principal;
		_identity  = identity;
	}

	public string Get()
	{
		var principal = _principal.Get();
		var subject   = principal.Identity as ClaimsIdentity ?? principal.Identities.AsValueEnumerable().First().Value;
		var identity  = _identity.Get(subject);
		var result    = _sign.Get(identity);
		return result;
	}
}