using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;

public sealed class DeviceTokenProcessStore : Variable<string>
{
    public static DeviceTokenProcessStore Default { get; } = new();

    DeviceTokenProcessStore() {}
}