using DragonSpark.Application.Model.Values;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;

public sealed class SaveDeviceToken : SaveState<string>
{
    public static SaveDeviceToken Default { get; } = new();

    SaveDeviceToken() : base(DeviceTokenProcessStore.Default, DeviceTokenStorage.Default) {}
}