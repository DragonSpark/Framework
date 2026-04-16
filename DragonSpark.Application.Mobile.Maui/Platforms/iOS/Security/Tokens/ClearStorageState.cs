using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;
using DragonSpark.Application.Mobile.Maui.Device.Security;
using DragonSpark.Application.Mobile.Maui.Security.Identity;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class ClearStorageState : IStopAware
{
    public static ClearStorageState Default { get; } = new();

    ClearStorageState() : this(ClearTokenState.Default, ClearSavedLogin.Default, ClearDeviceToken.Default) {}
 
    readonly Array<IDepending> _values;

    public ClearStorageState(params IDepending[] values) => _values = values;

    public async ValueTask Get(CancellationToken parameter)
    {
        foreach (var value in _values.Open())
        {
            await value.Off(parameter);
        }
    }
}