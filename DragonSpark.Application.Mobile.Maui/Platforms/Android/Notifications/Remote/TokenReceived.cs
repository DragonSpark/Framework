using Android.Gms.Tasks;
using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;
using DragonSpark.Model.Results;
using Java.Lang;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Notifications.Remote;

sealed class TokenReceived : Object, IOnSuccessListener
{
    public static TokenReceived Default { get; } = new();

    TokenReceived() : this(DeviceTokenProcessStore.Default) {}

    readonly IMutable<string?> _store;

    public TokenReceived(IMutable<string?> store) => _store = store;

    public void OnSuccess(Object? result)
    {
        _store.Execute(result?.ToString());
    }
}