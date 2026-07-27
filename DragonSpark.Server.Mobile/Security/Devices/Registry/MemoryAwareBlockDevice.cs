using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

sealed class MemoryAwareBlockDevice : IBlockDevice
{
    readonly Remove       _remove;
    readonly IBlockDevice _previous;

    public MemoryAwareBlockDevice(Remove remove, IBlockDevice previous)
    {
        _remove   = remove;
        _previous = previous;
    }

    public async ValueTask<bool> Get(Stop<BlockInput> parameter)
    {
        var ((deviceId, _), _) = parameter;
        var result = _previous.Off(parameter);
        _remove.Execute(deviceId);
        return await result;
    }
}