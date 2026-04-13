using DragonSpark.Application.Mobile.Configuration;
using DragonSpark.Application.Mobile.Maui.Run;
using DragonSpark.Application.Mobile.Maui.Runtime;
using DragonSpark.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Registrations = DragonSpark.Application.Security.Identity.Profile.Registrations;

namespace DragonSpark.Application.Mobile.Maui;

public static class Extensions
{
    public static BuildHostContext WithFrameworkConfigurations<T>(this BuildHostContext @this)
        => Configure<T>.Default.Get(@this);

    public static BuildHostContext WithRemoteConfiguration(this BuildHostContext @this)
        => @this.WithRemoteConfiguration<RemoteConfigurationMessage>();

    public static BuildHostContext WithRemoteConfiguration<T>(this BuildHostContext @this)
        where T : class, IRemoteConfigurationMessage
        => @this.Configure(Configuration.ApplyRemoteConfiguration<T>.Default.Adapt());

    public static IServiceCollection WithRemoteConfiguration(this IServiceCollection @this)
        => @this.WithRemoteConfiguration<RemoteConfigurationMessage>();

    public static IServiceCollection WithRemoteConfiguration<T>(this IServiceCollection @this)
        where T : class, IRemoteConfigurationMessage
        => Configuration.ApplyRemoteConfiguration<T>.Default.Adapt().Parameter(@this);

    public static BuildHostContext WithIdentityProfileFrameworkConfiguration(this BuildHostContext @this)
        => @this.Configure(Registrations.Default, Security.Identity.Registrations.Default);

    public static ICommand<T> AsMainThreadAware<T>(this ICommand<T> @this) => new MainThreadAwareCommand<T>(@this);
}