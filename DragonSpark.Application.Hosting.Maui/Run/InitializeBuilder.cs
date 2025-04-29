using System;
using System.Linq;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Hosting.Maui.Run;

sealed class InitializeBuilder(Func<IHostBuilder, IHostBuilder> host, Action<MauiAppBuilder> configure)
    : ISelect<MauiAppBuilder, MauiAppBuilder>
{
    public MauiAppBuilder Get(MauiAppBuilder parameter)
    {
        parameter.Services.AddSingleton(parameter);
        host(new MauiHostBuilder(parameter));
        configure(parameter);
        parameter.Services.Remove(parameter.Services.Single(x => x.ImplementationInstance == parameter));
        return parameter;
    }
}