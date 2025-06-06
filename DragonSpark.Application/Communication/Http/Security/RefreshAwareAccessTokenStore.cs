using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Diagnostics.Time;
using DragonSpark.Compose;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Communication.Http.Security;

public class RefreshAwareAccessTokenStore : IAccessTokenStore
{
    readonly IComposeTokenView _compose;
    readonly IAccessTokenStore _previous;
    readonly IWindow           _window;

    protected RefreshAwareAccessTokenStore(IComposeTokenView compose, IAccessTokenStore previous)
        : this(compose, previous, Time.Default.GreaterThan(TimeSpan.FromMinutes(30))) {}

    protected RefreshAwareAccessTokenStore(IComposeTokenView compose, IAccessTokenStore previous, IWindow window)
    {
        _compose  = compose;
        _previous = previous;
        _window   = window;
    }

    public async ValueTask<AccessTokenView?> Get(CancellationToken parameter)
    {
        var previous = await _previous.Off(parameter);
        return previous is null || _window.Get(previous.Expiration)
                   ? previous
                   : await _compose.Off(new(previous, parameter));
    }
}