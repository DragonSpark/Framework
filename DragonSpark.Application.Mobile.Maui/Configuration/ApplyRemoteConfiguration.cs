using DragonSpark.Application.Mobile.Configuration;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Mobile.Maui.Configuration;

sealed class ApplyRemoteConfiguration : ICommand<MauiAppBuilder>
{
    public static ApplyRemoteConfiguration Default { get; } = new();

    ApplyRemoteConfiguration() {}

    public void Execute(MauiAppBuilder parameter)
    {
        parameter.Services.Register<RemoteConfigurationSettings>();
        parameter.Configuration.AddRemoteConfiguration();
    }
}
