using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Hosting.Maui.Run;

public abstract class RunApplication(
    Func<MauiAppBuilder, MauiAppBuilder> builder,
    Func<MauiAppBuilder, MauiApp> application)
    : Mobile.Maui.Run.RunApplication(builder, application)
{
    protected RunApplication(Func<IHostBuilder, IHostBuilder> select, Action<MauiAppBuilder> configure)
        : this(select, configure, x => x.Build()) {}

    protected RunApplication(Func<IHostBuilder, IHostBuilder> select, Action<MauiAppBuilder> configure,
                             Func<MauiAppBuilder, MauiApp> host)
        : this(new InitializeBuilder(select, configure).Get, host) {}
}