using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Model.Operations.Results;
using Uno.Extensions;

namespace DragonSpark.Application.Mobile.Presentation.Models;

public class DispatchAwareToken<T> : IToken<T>
{
    readonly AsyncFunc<T> _previous;
    readonly IDispatcher  _dispatcher;

    public DispatchAwareToken(IToken<T> previous, IDispatcher dispatcher) : this(previous.Get, dispatcher) {}

    public DispatchAwareToken(AsyncFunc<T> previous, IDispatcher dispatcher)
    {
        _previous   = previous;
        _dispatcher = dispatcher;
    }

    public ValueTask<T> Get(CancellationToken parameter) => _dispatcher.ExecuteAsync(_previous, parameter);
}