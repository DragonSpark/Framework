using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using AndroidX.Core.App;
using DragonSpark.Application.Mobile.Device.Notifications;
using DragonSpark.Composition;
using DragonSpark.Model.Operations.Allocated;
using Uno.UI;

namespace DragonSpark.Application.Mobile.Android.Notifications;

sealed class NotificationManagerService : INotificationManagerService
{
    public event EventHandler NotificationReceived;

    int messageId, pendingIntentId;

    readonly NotificationManagerCompat _manager;
    readonly Context                   _context;
    readonly NotificationChannelView   _channel;
    
    public NotificationManagerService(NotificationChannelView channel) : this(BaseActivity.Current, channel) {}

    [Candidate(false)]
    public NotificationManagerService(Context context, NotificationChannelView channel)
        : this(NotificationManagerCompat.From(context), delegate {}, context, channel) {}

    // ReSharper disable once TooManyDependencies
    [Candidate(false)]
    public NotificationManagerService(NotificationManagerCompat manager, EventHandler received, Context context,
                                      NotificationChannelView channel)
    {
        _manager             = manager;
        _context             = context;
        _channel             = channel;
        NotificationReceived = received;
        CreateNotificationChannel();
    }

    void CreateNotificationChannel()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O &&
            _context.GetSystemService(Context.NotificationService) is NotificationManager manager)
        {
            var (identity, name, description, _) = _channel;
            var channelNameJava = new Java.Lang.String(name);
            var channel = new NotificationChannel(identity, channelNameJava, NotificationImportance.Default)
            {
                Description = description
            };

            manager.CreateNotificationChannel(channel);
        }
    }

    public Task SendNotification(Token<NotificationInput> notification)
    {
        var ((title, message, notifyTime), _) = notification;
        if (notifyTime is not null)
        {
            var intent = new Intent(_context, typeof(AlarmHandler));
            intent.PutExtra(TitleKey.Default, title);
            intent.PutExtra(MessageKey.Default, message);
            intent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTop);

            var flags = Build.VERSION.SdkInt >= BuildVersionCodes.S
                            ? PendingIntentFlags.CancelCurrent | PendingIntentFlags.Immutable
                            : PendingIntentFlags.CancelCurrent;

            var broadcast = PendingIntent.GetBroadcast(_context, pendingIntentId++, intent, flags);
            if (broadcast is not null)
            {
                var triggerTime = GetNotifyTime(notifyTime.Value);
                if (_context.GetSystemService(Context.AlarmService) is AlarmManager manager)
                {
                    manager.Set(AlarmType.RtcWakeup, triggerTime, broadcast);
                }
            }
        }
        else
        {
            Show(title, message);
        }

        return Task.CompletedTask;
    }

    public void ReceiveNotification(string title, string message)
    {
        NotificationReceived(this, new NotificationEventArgs(title, message));
    }

    void Show(string title, string message)
    {
        var intent = new Intent(_context, _context.GetType());
        intent.PutExtra(TitleKey.Default, title);
        intent.PutExtra(MessageKey.Default, message);
        intent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTop);

        var flags = Build.VERSION.SdkInt >= BuildVersionCodes.S
                        ? PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable
                        : PendingIntentFlags.UpdateCurrent;

        var (identity, _, _, (large, small)) = _channel;
        var activity = PendingIntent.GetActivity(_context, pendingIntentId++, intent, flags);
        var notification = new NotificationCompat.Builder(_context, identity)
                           .SetContentIntent(activity)
                           .SetContentTitle(title)
                           .SetContentText(message)
                           .SetLargeIcon(BitmapFactory.DecodeResource(_context.Resources, large))
                           .SetSmallIcon(small)
                           .Build();
        _manager.Notify(messageId++, notification);
    }

    long GetNotifyTime(DateTime notifyTime)
    {
        var utcTime   = TimeZoneInfo.ConvertTimeToUtc(notifyTime);
        var epochDiff = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;
        var result    = utcTime.AddSeconds(-epochDiff).Ticks / 10000; // milliseconds
        return result;
    }
}