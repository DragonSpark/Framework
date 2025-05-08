using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using Microsoft.Maui.Dispatching;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Models;

public class DispatchAwareResulting<T> : IResulting<T>
{
    readonly Func<Task<T>> _previous;
    readonly IDispatcher   _dispatcher;

    public DispatchAwareResulting(IResulting<T> previous, IDispatcher dispatcher)
        : this(previous.Allocate, dispatcher) {}

    public DispatchAwareResulting(Func<Task<T>> previous, IDispatcher dispatcher)
    {
        _previous   = previous;
        _dispatcher = dispatcher;
    }

    public ValueTask<T> Get()
    {
        var allocate = _dispatcher.IsDispatchRequired ? _dispatcher.DispatchAsync(_previous) : _previous();
        return allocate.ToOperation();
    }
}