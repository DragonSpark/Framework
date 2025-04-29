using DragonSpark.Composition.Compose;

namespace DragonSpark.Application.Mobile.Maui;

public static class Extensions
{
	public static BuildHostContext WithFrameworkConfigurations<T>(this BuildHostContext @this)
		=> Configure<T>.Default.Get(@this);

	public static BuildHostContext WithIdentityProfileFrameworkConfiguration(this BuildHostContext @this)
		=> @this.Configure(Security.Identity.Registrations.Default, Security.Identity.Profile.Registrations.Default);
    
}