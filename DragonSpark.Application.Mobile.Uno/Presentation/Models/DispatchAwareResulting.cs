using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using Uno.Extensions;

namespace DragonSpark.Application.Mobile.Uno.Presentation.Models;

public class DispatchAwareResulting<T> : IResulting<T>
{
    readonly IResulting<T> _previous;
    readonly IDispatcher   _dispatcher;

    public DispatchAwareResulting(IResulting<T> previous, IDispatcher dispatcher)
    {
        _previous   = previous;
        _dispatcher = dispatcher;
    }

    public ValueTask<T> Get() => _dispatcher.ExecuteAsync<T>(async _ => await _previous.Off());
}