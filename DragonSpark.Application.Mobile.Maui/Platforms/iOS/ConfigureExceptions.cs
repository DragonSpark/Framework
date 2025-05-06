using System;
using DragonSpark.Application.Mobile.Maui.Diagnostics;
using ObjCRuntime;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS;

sealed class ConfigureExceptions : IConfigureExceptions
{
    readonly IConfigureExceptions        _previous;
    readonly MarshalManagedExceptionMode _mode;

    public ConfigureExceptions(IConfigureExceptions previous)
        : this(previous, MarshalManagedExceptionMode.UnwindNativeCode) {}

    public ConfigureExceptions(IConfigureExceptions previous, MarshalManagedExceptionMode mode)
    {
        _previous = previous;
        _mode     = mode;
    }

    public void Execute(UnhandledExceptionEventHandler parameter)
    {
        _previous.Execute(parameter);

        // For iOS and Mac Catalyst
        // Exceptions will flow through AppDomain.CurrentDomain.UnhandledException,
        // but we need to set UnwindNativeCode to get it to work correctly.
        //
        // See: https://github.com/xamarin/xamarin-macios/issues/15252

        ObjCRuntime.Runtime.MarshalManagedException += (_, args) => args.ExceptionMode = _mode;
    }
}