using System;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Mobile.Maui.Run;

public abstract class RunApplication(
    Func<MauiAppBuilder, MauiAppBuilder> builder,
    Func<MauiAppBuilder, MauiApp> application)
    : IRunApplication
{
    readonly Func<MauiAppBuilder, MauiAppBuilder> _builder     = builder;
    readonly Func<MauiAppBuilder, MauiApp>        _application = application;

    public MauiApp Get(MauiAppBuilder parameter)
    {
        var builder = _builder(parameter);
        return _application(builder);
    }
}