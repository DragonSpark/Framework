using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Maui.Device.Security;
using DragonSpark.Application.Mobile.Maui.Security.Identity;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class ClearStorageState : IStopAware
{
    public static ClearStorageState Default { get; } = new();

    ClearStorageState() : this(DeviceKeyProcessStore.Default, ClearTokenState.Default, ClearSavedLogin.Default) {} // TODO: Clear device token

    readonly IMutable<PublicJWK?> _process;
    readonly IDepending           _token;
    readonly IDepending           _login;

    public ClearStorageState(IMutable<PublicJWK?> process, IDepending token, IDepending login)
    {
        _process = process;
        _token   = token;
        _login   = login;
    }

    public async ValueTask Get(CancellationToken parameter)
    {
        _process.Execute(null);
        await _token.Off(parameter);
        await _login.Off(parameter);
    }
}