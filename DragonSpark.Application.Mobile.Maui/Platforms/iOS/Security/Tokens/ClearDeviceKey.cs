using System.Threading.Tasks;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Results;
using Security;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class ClearDeviceKey : IClearDeviceKey
{
    public static ClearDeviceKey Default { get; } = new();

    ClearDeviceKey() : this(SecurityRecord.Default, DeviceKeyProcessStore.Default, DeviceKeyStorageValue.Default) {}

    readonly SecRecord            _record;
    readonly IMutable<PublicJWK?> _process;
    readonly IDepending           _store;

    public ClearDeviceKey(SecRecord record, IMutable<PublicJWK?> process, IDepending store)
    {
        _record  = record;
        _process = process;
        _store   = store;
    }

    public async ValueTask<bool> Get(Stop<None> parameter)
    {
        _process.Execute(null);
        await _store.Off(None.Default.Stop(parameter));
        var status = SecKeyChain.Remove(_record);
        var result = status == SecStatusCode.Success;
        return result;
    }
}