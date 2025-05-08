using System;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Maui.Device.Notifications;
using DragonSpark.Model.Operations.Allocated;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Notifications;

public class Notifications : INotifications
{
    public event EventHandler? NotificationReceived;

    readonly ISend _send;

    public Notifications() : this(PermissionAwareSend.Default) {}

    public Notifications(ISend send) => _send = send;

    public Task SendNotification(Token<NotificationInput> notification) => _send.Get(notification);

    public void ReceiveNotification(string title, string message)
    {
        var args = new NotificationEventArgs(title, message);
        NotificationReceived?.Invoke(null, args);
    }
}