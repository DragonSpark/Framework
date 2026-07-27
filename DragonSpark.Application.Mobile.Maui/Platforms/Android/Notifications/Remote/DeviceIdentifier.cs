using Android.Content;
using Android.Provider;
using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;
using DragonSpark.Compose;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Notifications.Remote;

sealed class DeviceIdentifier : IDeviceIdentifier
{
    public static DeviceIdentifier Default { get; } = new();

    DeviceIdentifier() : this(Platform.AppContext.ContentResolver.Verify(), Settings.Secure.AndroidId) {}

    readonly ContentResolver _resolver;
    readonly string          _key;

    public DeviceIdentifier(ContentResolver resolver, string key)
    {
        _resolver = resolver;
        _key      = key;
    }

    public string Get() => Settings.Secure.GetString(_resolver, _key).Verify();
}