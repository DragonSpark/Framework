using System;
using DragonSpark.Model.Selection;
using Foundation;
using UserNotifications;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Notifications;

sealed class DetermineTrigger : ISelect<DateTime?, UNNotificationTrigger>
{
    public static DetermineTrigger Default { get; } = new();

    DetermineTrigger() : this(ConvertToNSDateComponents.Default) {}

    readonly ISelect<DateTime, NSDateComponents> _date;

    public DetermineTrigger(ISelect<DateTime, NSDateComponents> date) => _date = date;

    public UNNotificationTrigger Get(DateTime? parameter)
        => parameter is not null
               // Create a calendar-based trigger.
               ? UNCalendarNotificationTrigger.CreateTrigger(_date.Get(parameter.Value), false)
               :
               // Create a time-based trigger, interval is in seconds and must be greater than 0.
               UNTimeIntervalNotificationTrigger.CreateTrigger(0.25, false);
}