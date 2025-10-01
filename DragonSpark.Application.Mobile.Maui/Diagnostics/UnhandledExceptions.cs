using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

sealed class UnhandledExceptions : IMauiInitializeService
{
    public static UnhandledExceptions Default { get; } = new();

    UnhandledExceptions() {}

    public void Initialize(IServiceProvider services)
    {
        services.GetRequiredService<IConfigureExceptions>().Execute(new HandleExceptions(services).Execute);
    }
}