using DragonSpark.Compose;
using DragonSpark.Contracts.Security;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Communication.Http.Security;

sealed class ComposeTokenView : IComposeTokenView
{
    readonly Func<IRefreshTokenResponse> _refresh;
    readonly IUpdateTokenState           _update;

    public ComposeTokenView(Func<IRefreshTokenResponse> refresh, IUpdateTokenState update)
    {
        _refresh = refresh;
        _update  = update;
    }

    public async ValueTask<AccessTokenView?> Get(Stop<AccessTokenView> parameter)
    {
        var ((identifier, _, response), stop) = parameter;
        var refresh = await _refresh().On(new(response, stop));
        var result = refresh is not null ? new AccessTokenView(identifier, refresh) : null;
        await _update.Off(new(result, stop));
        return result;
    }
}