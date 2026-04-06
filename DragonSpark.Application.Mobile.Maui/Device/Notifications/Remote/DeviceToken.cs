using DragonSpark.Model.Operations.Results.Stop;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;

public sealed class DeviceToken : Storing<string?>
{
    public static DeviceToken Default { get; } = new();

    DeviceToken() : base(DeviceTokenProcessStore.Default, DeviceTokenStorage.Default) {}
}