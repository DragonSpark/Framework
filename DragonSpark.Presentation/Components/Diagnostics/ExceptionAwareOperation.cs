using System;
using System.Threading.Tasks;
using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Components.Diagnostics;

sealed class ExceptionAwareOperation : IOperation
{
    readonly Type        _owner;
    readonly IExceptions _exceptions;
    readonly Func<Task>  _callback;

    public ExceptionAwareOperation(Type owner, IExceptions exceptions, Func<Task> callback)
    {
        _owner      = owner;
        _exceptions = exceptions;
        _callback   = callback;
    }

    public async ValueTask Get()
    {
        try
        {
            await _callback().Off();
        }
        // ReSharper disable once CatchAllClause
        catch (Exception e)
        {
            await _exceptions.Off(new(_owner, e));
        }
    }
}

sealed class ExceptionAwareOperation<T> : IOperation<T>
{
    readonly Type          _owner;
    readonly IExceptions   _exceptions;
    readonly Func<T, Task> _callback;

    public ExceptionAwareOperation(Type owner, IExceptions exceptions, Func<T, Task> callback)
    {
        _owner      = owner;
        _exceptions = exceptions;
        _callback   = callback;
    }

    public async ValueTask Get(T parameter)
    {
        try
        {
            await _callback(parameter).Off();
        }
        // ReSharper disable once CatchAllClause
        catch (Exception e)
        {
            await _exceptions.Off(new(_owner, e));
        }
    }
}