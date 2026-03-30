using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class StateAwareClearDeviceKey : IClearDeviceKey
{
    readonly IClearDeviceKey      _previous;
    readonly IStopAware           _clear;

    public StateAwareClearDeviceKey(IClearDeviceKey previous) : this(previous, ClearStorageState.Default) {}

    public StateAwareClearDeviceKey(IClearDeviceKey previous, IStopAware clear)
    {
        _previous = previous;
        _clear    = clear;
    }

    public async ValueTask<bool> Get(Stop<None> parameter)
    {
        await _clear.Off(parameter);
        return await _previous.Off(parameter);
    }
}