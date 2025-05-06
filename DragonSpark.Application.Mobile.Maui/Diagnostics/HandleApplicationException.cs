using System;
using DragonSpark.Application.Mobile.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

sealed class HandleApplicationException : ICommand<HandleApplicationExceptionInput>
{
    readonly IApplicationErrorHandler _handler;

    public HandleApplicationException(IApplicationErrorHandler handler) => _handler = handler;

    public void Execute(object sender, object exception)
    {
        Execute(new(sender, exception));
    }

    public void Execute(HandleApplicationExceptionInput parameter)
    {
        var (sender, @object) = parameter;
        if ((@object is UnhandledExceptionEventArgs args ? args.ExceptionObject : @object) is Exception exception)
        {
            _handler.Execute(exception);
            if (sender is IPlatformApplication application)
            {
                application.Services.GetService<IFlushLogging>()?.Execute();
            }
        }
    }
}