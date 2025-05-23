using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Communication.Http.Security;

sealed class ComposeTokenView : IComposeTokenView
{
    readonly IRefreshTokenResponse _refresh;
    readonly IAccessTokenView      _challenge;

    public ComposeTokenView(IRefreshTokenResponse refresh, IAccessTokenView challenge)
    {
        _refresh   = refresh;
        _challenge = challenge;
    }

    public async ValueTask<AccessTokenView?> Get(Stop<AccessTokenView> parameter)
    {
        var ((identifier, _, response), stop) = parameter;
        var refresh = await _refresh.Off(new(response, stop));
        return refresh is not null
                   ? new(identifier, refresh)
                   : await _challenge.Off(new(new(identifier), stop)) is {} r
                       ? new(identifier, r)
                       : null;
    }
}