using DragonSpark.Application.Mobile.Maui.Run;
using DragonSpark.Application.Security.Identity.Profile;
using DragonSpark.Composition.Compose;

namespace DragonSpark.Application.Mobile.Maui;

public static class Extensions
{
    public static BuildHostContext WithFrameworkConfigurations<T>(this BuildHostContext @this)
        => Configure<T>.Default.Get(@this);

    public static BuildHostContext WithRemoteConfiguration(this BuildHostContext @this)
        => @this.Configure(Configuration.ApplyRemoteConfiguration.Default.Adapt());

    public static BuildHostContext WithIdentityProfileFrameworkConfiguration(this BuildHostContext @this)
        => @this.Configure(Registrations.Default, Security.Identity.Registrations.Default);
}