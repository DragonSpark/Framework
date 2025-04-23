using System.Threading.Tasks;
using Android;
using DragonSpark.Application.Mobile.Uno.Device.Messaging;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;
using Windows.Extensions;

namespace DragonSpark.Application.Mobile.Uno.Platforms.Android.Messages;

sealed class PermissionAwareMessenger : IMessenger
{
    readonly IMessenger _previous;
    readonly string     _permission;

    public PermissionAwareMessenger(IMessenger previous, string permission = Manifest.Permission.SendSms)
    {
        _previous   = previous;
        _permission = permission;
    }

    public async ValueTask<bool> Get(Token<MessageInput> parameter)
    {
        var (_, item) = parameter;
        var granted = await PermissionsHelper.TryGetPermission(item, _permission).Off();
        var result  = granted && await _previous.Off(parameter);
        return result;
    }
}