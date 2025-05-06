using System;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

sealed class DefaultConfigureExceptions : IConfigureExceptions
{
    public static DefaultConfigureExceptions Default { get; } = new();

    DefaultConfigureExceptions() : this(AppDomain.CurrentDomain) {}

    readonly AppDomain _domain;

    public DefaultConfigureExceptions(AppDomain domain) => _domain = domain;

    public void Execute(UnhandledExceptionEventHandler parameter)
    {
        _domain.UnhandledException += parameter;
    }
}