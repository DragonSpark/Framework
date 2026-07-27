using DragonSpark.Application.AspNet.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Server.Mobile.Security.Devices.Authentication;

namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

sealed class DeviceAwareMarkUsed : IMarkUsed
{
    readonly IMarkUsed     _previous;
    readonly IDeviceSeen   _device;
    readonly CurrentDevice _current;

    public DeviceAwareMarkUsed(IMarkUsed previous, IDeviceSeen device, CurrentDevice current)
    {
        _previous = previous;
        _device   = device;
        _current  = current;
    }

    public async ValueTask<bool> Get(Stop<string> parameter)
        => await _previous.Off(parameter) | await _device.Off(_current.Get().Stop(parameter));
}