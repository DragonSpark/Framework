using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class ClearDeviceState : IStopAware
{
    readonly ITokens              _tokens;
    readonly IMutable<PublicJWK?> _process;
    readonly IDepending           _store;

    public ClearDeviceState(ITokens tokens)
        : this(tokens, DeviceKeyProcessStore.Default, DeviceKeyStorageValue.Default) {}

    public ClearDeviceState(ITokens tokens, IMutable<PublicJWK?> process, IDepending store)
    {
        _tokens  = tokens;
        _process = process;
        _store   = store;
    }

    public async ValueTask Get(CancellationToken parameter)
    {
        _tokens.Execute();
        _process.Execute(null);
        await _store.Off(None.Default.Stop(parameter));
    }
}