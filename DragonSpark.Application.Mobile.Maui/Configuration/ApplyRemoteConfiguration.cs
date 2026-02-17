using DragonSpark.Application.Mobile.Attestation;
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
        parameter.Services.AddSingleton<IMauiInitializeService>(InitializeRemoteConfiguration.Default)
                 .Register<RemoteConfigurationSettings>()
                 .AddSingleton<IRemoteConfigurationMessage, T>()
                 //
                 .Start<T>()
                 .Include(x => x.Dependencies.Recursive())
                 .Singleton()
                 //
                 .Then.Start<IValidationIdentity>()
                 .Forward<ValidationIdentity>()
                 .Singleton()
                 //
                 .Then.Start<IExistingAttestation>().Forward<ExistingAttestation>().Singleton()
                 //
                 .Then.Start<IClearValidationIdentity>()
                 .Forward<ClearValidationIdentity>()
                 .Singleton()
                 //
                 .Then.Start<SaveIdentity>()
                 .Include(x => x.Dependencies)
                 .Singleton()
                 //
                 .Then.AddRemoteConfiguration(parameter.Configuration);
    }
}