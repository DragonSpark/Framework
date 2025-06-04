using System.Net.Http;
using System.Threading.Tasks;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Application.Security.Identity.Bearer;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.Communication.Http;

sealed class CurrentBearer : IStopAware<HttpRequestMessage, string?>
{
	readonly ICurrentPrincipal _principal;
	readonly IBearer           _bearer;

	public CurrentBearer(IBearer bearer) : this(CurrentPrincipal.Default, bearer) {}

	public CurrentBearer(ICurrentPrincipal principal, IBearer bearer)
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

sealed class BearerAwareAccessTokenProvider : Maybe<Stop<HttpRequestMessage>, string>, IAccessTokenProvider
{
    public BearerAwareAccessTokenProvider(IAccessTokenProvider previous, CurrentBearer current)
        : base(previous, current) {}
}