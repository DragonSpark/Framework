using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;
using UIKit;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Notifications.Remote;

sealed class IsSupported : ICondition
{
    public static IsSupported Default { get; } = new();

    IsSupported() : this(UIDevice.CurrentDevice, 13, 0) {}

    readonly UIDevice _device;
    readonly byte     _major;
    readonly byte     _minor;

    public IsSupported(UIDevice device, byte major, byte minor)
    {
        _device = device;
        _major  = major;
        _minor  = minor;
    }

    public bool Get(None parameter) => _device.CheckSystemVersion(_major, _minor);
}