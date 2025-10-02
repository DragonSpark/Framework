using System;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

sealed class HandleExceptions
{
    readonly IServiceProvider _services;

    public HandleExceptions(IServiceProvider services) => _services = services;

    public void Execute(object sender, UnhandledExceptionEventArgs e)
    {
        _services.GetRequiredService<HandleApplicationException>().Execute(sender, e);
    }
}