using System.Threading.Tasks;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class StateAwareClearDeviceKey : IClearDeviceKey
{
    readonly IClearDeviceKey      _previous;
    readonly IMutable<PublicJWK?> _process;

    public StateAwareClearDeviceKey(IClearDeviceKey previous) : this(previous, DeviceKeyProcessStore.Default) {}

    public StateAwareClearDeviceKey(IClearDeviceKey previous, IMutable<PublicJWK?> process)
    {
        _previous = previous;
        _process  = process;
    }

    public async ValueTask<bool> Get(Stop<None> parameter)
    {
        _process.Execute(null);
        return await _previous.Off(parameter);
    }
}