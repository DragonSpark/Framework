using System;
using DragonSpark.Application.Mobile.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

sealed class HandleApplicationException : ICommand<HandleApplicationExceptionInput>
{
    readonly IApplicationErrorHandler _handler;
    readonly Func<IFlushLogging?>     _flush;

    public HandleApplicationException(IApplicationErrorHandler handler, Func<IFlushLogging?> flush)
    {
        _handler = handler;
        _flush   = flush;
    }

    public void Execute(object sender, object exception)
    {
        Execute(new(sender, exception));
    }

    public void Execute(HandleApplicationExceptionInput parameter)
    {
        var (_, @object) = parameter;
        if ((@object is UnhandledExceptionEventArgs args ? args.ExceptionObject : @object) is Exception exception)
        {
            _handler.Execute(exception);
            _flush()?.Execute();
        }
    }
}