using DragonSpark.Application.Mobile.Configuration;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Mobile.Maui.Configuration;

sealed class ApplyRemoteConfiguration<T> : ICommand<MauiAppBuilder> where T : class, IRemoteConfigurationMessage
{
    public static ApplyRemoteConfiguration<T> Default { get; } = new();

    ApplyRemoteConfiguration() {}

    public void Execute(MauiAppBuilder parameter)
    {
        parameter.Services.Register<RemoteConfigurationSettings>()
                 .AddSingleton<IRemoteConfigurationMessage, T>()
                 //
                 .Start<T>()
                 .Include(x => x.Dependencies.Recursive())
                 .Singleton()
                 //
                 .Then.AddRemoteConfiguration(parameter.Configuration);
    }
}