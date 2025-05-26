using System.Net.Http;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Results.Stop;

namespace DragonSpark.Application.Communication.Http.Security;

public class AccessTokenProvider : IAccessTokenProvider
{
    readonly IAccessTokenProvider         _previous;
    readonly IStopAware<AccessTokenView?> _view;

    protected AccessTokenProvider(IAccessTokenProvider previous, IAccessTokenStore view)
    {
        _previous = previous;
        _view     = view;
    }

    public async ValueTask<string?> Get(Stop<HttpRequestMessage> parameter)
    {
        var view   = await _view.Off(parameter);
        var result = view?.Response.AccessToken ?? await _previous.Off(parameter);
        return result;
    }
}