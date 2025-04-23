using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Uno.Device.Notifications;
using DragonSpark.Model.Operations.Allocated;
using UserNotifications;

namespace DragonSpark.Application.Mobile.Uno.Platforms.iOS.Notifications;

sealed class PermissionAwareSend : ISend
{
    public static PermissionAwareSend Default { get; } = new();

    PermissionAwareSend() : this(Send.Default, UNUserNotificationCenter.Current) {}

    readonly ISend _previous;
    bool           _allowed;

    public PermissionAwareSend(ISend previous, UNUserNotificationCenter center)
    {
        _previous = previous;

        // Request permission to use local notifications.
        center.RequestAuthorization(UNAuthorizationOptions.Alert, (approved, _) => _allowed = approved);
    }

    public Task Get(Token<NotificationInput> parameter) => _allowed ? _previous.Get(parameter) : Task.CompletedTask;
}