using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Communication.Http.Security;

sealed class ComposeTokenView : IComposeTokenView
{
    readonly IRefreshTokenResponse _refresh;
    readonly IChallenge            _challenge;
    readonly IUpdateTokenState     _update;

    public ComposeTokenView(IRefreshTokenResponse refresh, IChallenge challenge, IUpdateTokenState update)
    {
        _refresh   = refresh;
        _challenge = challenge;
        _update    = update;
    }

    public async ValueTask<AccessTokenView?> Get(Stop<AccessTokenView> parameter)
    {
        var ((identifier, _, response), stop) = parameter;
        var refresh = await _refresh.On(new(response, stop));
        var result = refresh is not null
                         ? new AccessTokenView(identifier, refresh)
                         : await _challenge.Off(new(new(identifier), stop)) is {} r
                             ? new(identifier, r)
                             : null;
        await _update.Off(new(result, stop));
        return result;
    }
}