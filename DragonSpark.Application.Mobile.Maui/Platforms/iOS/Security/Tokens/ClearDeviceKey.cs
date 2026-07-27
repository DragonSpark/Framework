using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using Security;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class ClearDeviceKey : IClearDeviceKey
{
    public static ClearDeviceKey Default { get; } = new();

    ClearDeviceKey() : this(SecurityRecord.Default) {}

    readonly SecRecord _record;

    public ClearDeviceKey(SecRecord record) => _record = record;

    public ValueTask<bool> Get(Stop<None> parameter)
    {
        var result = SecKeyChain.Remove(_record) == SecStatusCode.Success;
        return result.ToOperation();
    }
}