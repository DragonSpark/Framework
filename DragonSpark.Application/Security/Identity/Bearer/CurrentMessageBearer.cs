using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using System.Net.Http;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class CurrentMessageBearer : IStopAware<HttpRequestMessage, string?>
{
	readonly ICurrentPrincipal _principal;
	readonly IBearer           _bearer;

	public CurrentMessageBearer(IBearer bearer) : this(CurrentPrincipal.Default, bearer) {}

	public CurrentMessageBearer(ICurrentPrincipal principal, IBearer bearer)
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