using System;
using DragonSpark.Application.Mobile.Runtime.Initialization;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Hosting.Maui.Run;

sealed class BuildApplication : ISelect<MauiAppBuilder, MauiApp>
{
    public static BuildApplication Default { get; } = new();

    BuildApplication() : this(AssignInitializationServices.Default) {}

    readonly ICommand<IServiceProvider> _provider;

    public BuildApplication(ICommand<IServiceProvider> provider) => _provider = provider;

    public MauiApp Get(MauiAppBuilder parameter)
    {
        var result = parameter.Build();
        _provider.Execute(result.Services);
        return result;
    }
}