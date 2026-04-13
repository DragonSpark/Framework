using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;
using DragonSpark.Compose;
using UIKit;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Notifications.Remote;

sealed class DeviceIdentifier : IDeviceIdentifier
{
    public static DeviceIdentifier Default { get; } = new();

    DeviceIdentifier() : this(UIDevice.CurrentDevice) {}

    readonly UIDevice _device;

    public DeviceIdentifier(UIDevice device) => _device = device;

    public string Get() => _device.IdentifierForVendor.Verify().ToString();
}