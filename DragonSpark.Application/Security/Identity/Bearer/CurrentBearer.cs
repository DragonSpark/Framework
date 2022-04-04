using NetFabric.Hyperlinq;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class CurrentBearer : ICurrentBearer
{
	readonly ISign             _sign;
	readonly ICurrentPrincipal _principal;

	public CurrentBearer(ISign sign, ICurrentPrincipal principal)
	{
		_sign      = sign;
		_principal = principal;
	}

	public string Get()
	{
		var principal = _principal.Get();
		var subject   = principal.Identity as ClaimsIdentity ?? principal.Identities.AsValueEnumerable().First().Value;
		var result    = _sign.Get(subject);
		return result;
	}
}