using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model;

namespace DragonSpark.Application.Mobile.Maui.Device.Security;

sealed class SimulatorAwareSupportsPasskey : ISupportsPasskey
{
    readonly IIsSimulator     _simulator;
    readonly ISupportsPasskey _previous;

    public SimulatorAwareSupportsPasskey(IIsSimulator simulator, ISupportsPasskey previous)
    {
        _simulator = simulator;
        _previous  = previous;
    }

    public ValueTask<bool> Get(None parameter) => _simulator.Get() ? false.ToOperation() : _previous.Get(parameter);
}