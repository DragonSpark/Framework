using DragonSpark.Application.Model.Values;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;

public sealed class ClearDeviceToken : ClearState<string>
{
    public static ClearDeviceToken Default { get; } = new();

    ClearDeviceToken() : base(DeviceTokenProcessStore.Default, DeviceTokenStorage.Default) {}
}