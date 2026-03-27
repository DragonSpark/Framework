using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Maui.Security.Identity;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class StateAwareClearDeviceKey : IClearDeviceKey
{
    readonly IClearDeviceKey      _previous;
    readonly IMutable<PublicJWK?> _process;
    readonly IDepending           _token;

    public StateAwareClearDeviceKey(IClearDeviceKey previous)
        : this(previous, DeviceKeyProcessStore.Default, ClearTokenState.Default) {}

    public StateAwareClearDeviceKey(IClearDeviceKey previous, IMutable<PublicJWK?> process, IDepending token)
    {
        _previous   = previous;
        _process    = process;
        _token = token;
    }

    public async ValueTask<bool> Get(Stop<None> parameter)
    {
        _process.Execute(null);
        await _token.Off(parameter);
        return await _previous.Off(parameter);
    }
}