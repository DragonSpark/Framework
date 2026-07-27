namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;

sealed class InitializeDeviceRegistration : IMauiInitializeService
{
    public static InitializeDeviceRegistration Default { get; } = new();

    InitializeDeviceRegistration() {}

    public void Initialize(IServiceProvider services)
    {
        _ = services.GetRequiredService<DeviceRegistration>().Get(CancellationToken.None);
    }
}