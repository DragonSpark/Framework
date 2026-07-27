using Android.Content;
using DragonSpark.Application.Mobile.Maui.Device.Notifications;
using DragonSpark.Application.Mobile.Maui.Presentation;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Notifications;

public sealed class ReceiveNotification : ICommand<Intent>
{
    public static ReceiveNotification Default { get; } = new();

    ReceiveNotification()
        : this(CurrentServices.Default.GetService<INotifications>, TitleKey.Default, MessageKey.Default) {}

    readonly Func<INotifications?> _notifications;
    readonly string               _title, _message;

    public ReceiveNotification(Func<INotifications?> notifications, string title, string message)
    {
        _notifications = notifications;
        _title         = title;
        _message       = message;
    }

    public void Execute(Intent parameter)
    {
        var service = _notifications();
        if (service is not null)
        {
            var title   = parameter.GetStringExtra(_title);
            var message = parameter.GetStringExtra(_message);
            if (title is not null && message is not null)
            {
                service.ReceiveNotification(title, message);
            }
        }
    }
}