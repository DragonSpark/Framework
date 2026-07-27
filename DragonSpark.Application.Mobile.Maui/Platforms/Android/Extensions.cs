using DragonSpark.Compose;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android;

public static class Extensions
{
    extension(IServiceCollection @this)
    {
        public IServiceCollection RegisterFrameworkServices() => Registrations.Default.Parameter(@this);

        public IServiceCollection WithPushNotifications()
            => Notifications.Remote.Registrations.Default.Parameter(@this);

        public IServiceCollection WithHttp() => Http.Registrations.Default.Parameter(@this);

        public IServiceCollection WithVerification() => Attestation.Registrations.Default.Parameter(@this);

        public IServiceCollection WithDeviceAuthorization() => Security.Tokens.Registrations.Default.Parameter(@this);
    }
}