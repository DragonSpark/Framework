using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

sealed class UnhandledExceptions : IMauiInitializeService
{
    readonly UnhandledExceptionEventHandler _handler;

    public UnhandledExceptions(HandleApplicationException handle) : this(handle.Execute) {}

    public UnhandledExceptions(UnhandledExceptionEventHandler handler) => _handler = handler;

    public void Initialize(IServiceProvider services)
    {
        services.GetRequiredService<IConfigureExceptions>().Execute(_handler);
    }
}