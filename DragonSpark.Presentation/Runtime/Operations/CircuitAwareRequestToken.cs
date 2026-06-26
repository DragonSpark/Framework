using System;
using System.Threading;
using DragonSpark.Presentation.Components.Content.Rendering;

namespace DragonSpark.Presentation.Runtime.Operations;

sealed class CircuitAwareRequestToken : IRequestToken, IDisposable
{
    readonly IRequestToken       _previous;
    readonly IRenderState        _state;
    readonly CircuitRequestToken _request;

    public CircuitAwareRequestToken(IRequestToken previous, IRenderState state, CircuitRequestToken request)
    {
        _previous = previous;
        _state    = state;
        _request  = request;
    }

    public CancellationToken Get()
    {
        var source = _state.IsConnected() ? _request : _previous;
        var result = source.Get();
        return result;
    }

    public void Dispose()
    {
        _request.Dispose();
    }
}