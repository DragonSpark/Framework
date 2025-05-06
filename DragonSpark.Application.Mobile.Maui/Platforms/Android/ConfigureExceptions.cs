using System;
using Android.Runtime;
using DragonSpark.Application.Mobile.Maui.Diagnostics;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android;

sealed class ConfigureExceptions : IConfigureExceptions
{
    readonly IConfigureExceptions _previous;

    public ConfigureExceptions(IConfigureExceptions previous) => _previous = previous;

    public void Execute(UnhandledExceptionEventHandler parameter)
    {
        _previous.Execute(parameter);

        // For Android:
        // All exceptions will flow through Android.Runtime.AndroidEnvironment.UnhandledExceptionRaiser,
        // and NOT through AppDomain.CurrentDomain.UnhandledException

        AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) => parameter(sender!, new(args.Exception, true));
    }
}