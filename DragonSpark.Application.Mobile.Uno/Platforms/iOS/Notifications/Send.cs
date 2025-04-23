using System;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Uno.Device.Notifications;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Selection;
using Foundation;
using UserNotifications;

namespace DragonSpark.Application.Mobile.Uno.Platforms.iOS.Notifications;

sealed class Send : ISend
{
    public static Send Default { get; } = new();

    Send() : this(DetermineTrigger.Default, UNUserNotificationCenter.Current, Handle.Default.Execute) {}

    readonly ISelect<DateTime?, UNNotificationTrigger> _trigger;
    readonly UNUserNotificationCenter                  _center;
    readonly Action<NSError?>                          _handle;
    int                                                _identity;

    public Send(ISelect<DateTime?, UNNotificationTrigger> trigger, UNUserNotificationCenter center,
                Action<NSError?> handle)
    {
        _trigger = trigger;
        _center  = center;
        _handle  = handle;
        // Create a UNUserNotificationCenterDelegate to handle incoming messages.
        _center.Delegate = new NotificationReceiver();
    }

    public Task Get(Token<NotificationInput> parameter)
    {
        var ((title, message, notifyTime), _) = parameter;

        var content = new UNMutableNotificationContent
        {
            Title    = title,
            Subtitle = string.Empty,
            Body     = message,
            Badge    = 1
        };

        var trigger = _trigger.Get(notifyTime);
        var request = UNNotificationRequest.FromIdentifier((_identity++).ToString(), content, trigger);
        _center.AddNotificationRequest(request, _handle);

        return Task.CompletedTask;
    }
}