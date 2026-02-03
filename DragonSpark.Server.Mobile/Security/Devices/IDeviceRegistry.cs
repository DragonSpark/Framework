using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Server.Mobile.Security.Devices;

public interface IDeviceRegistry : IStopAware<string, DeviceRecord?>;

// TODO

sealed class ProofAwareDeviceRegistry : IDeviceRegistry
{
    readonly IDeviceRegistry _previous;

    public ProofAwareDeviceRegistry(IDeviceRegistry previous)
    {
        _previous = previous;
    }

    public async ValueTask<DeviceRecord?> Get(Stop<string> parameter)
    {
        var previous = await _previous.Off(parameter);
        if (previous is null)
        {

        }

        return previous;
    }
}