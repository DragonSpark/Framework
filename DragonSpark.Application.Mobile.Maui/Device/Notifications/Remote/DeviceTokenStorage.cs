using DragonSpark.Application.Mobile.Maui.Storage;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;

sealed class DeviceTokenStorage : StorageValue<string>
{
    public static DeviceTokenStorage Default { get; } = new();

    DeviceTokenStorage() {}
}