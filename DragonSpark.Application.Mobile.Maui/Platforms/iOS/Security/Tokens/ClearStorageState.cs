using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;
using DragonSpark.Application.Mobile.Maui.Device.Security;
using DragonSpark.Application.Mobile.Maui.Security.Identity;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Stop.Conditions;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class ClearStorageState : IStopAware
{
    public static ClearStorageState Default { get; } = new();

    ClearStorageState()
        : this(DeviceKeyProcessStore.Default, ClearTokenState.Default, ClearSavedLogin.Default,
               ClearDeviceToken.Default) {}

    readonly IMutable<PublicJWK?> _store;
    readonly Array<IDepending>    _values;

    public ClearStorageState(IMutable<PublicJWK?> store, params IDepending[] values)
    {
        _store  = store;
        _values = values;
    }

    public async ValueTask Get(CancellationToken parameter)
    {
        _store.Execute(null);
        foreach (var value in _values.Open())
        {
            await value.Off(parameter);
        }
    }
}