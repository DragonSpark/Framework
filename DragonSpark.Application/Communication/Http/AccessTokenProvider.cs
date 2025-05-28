using DragonSpark.Application.Security.Identity;
using DragonSpark.Application.Security.Identity.Bearer;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Net.Http;
using System.Threading.Tasks;

namespace DragonSpark.Application.Communication.Http;

sealed class AccessTokenProvider : IAccessTokenProvider
{
	readonly ICurrentPrincipal _principal;
	readonly IBearer           _bearer;

	public AccessTokenProvider(IBearer bearer) : this(CurrentPrincipal.Default, bearer) {}

	public AccessTokenProvider(ICurrentPrincipal principal, IBearer bearer)
	{
		_principal = principal;
		_bearer    = bearer;
	}

	public ValueTask<string?> Get(Stop<HttpRequestMessage> parameter)
	{
		var principal = _principal.Get();
		var bearer    = principal.IsAuthenticated() ? _bearer.Get(principal) : null;
		return bearer.ToOperation();
	}
}