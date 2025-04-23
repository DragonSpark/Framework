using System;
using Android.Content;
using Android.Runtime;
using DragonSpark.Application.Mobile.Uno.Device.Notifications;
using DragonSpark.Application.Mobile.Uno.Presentation;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Uno.Platforms.Android.Notifications;

public abstract class AlarmHandler : BroadcastReceiver
{
    readonly IServiceProvider            _services;

    protected AlarmHandler() : this(CurrentServices.Default) {}

    protected AlarmHandler(IServiceProvider services) => _services = services;

    protected AlarmHandler(IntPtr javaReference, JniHandleOwnership transfer, IServiceProvider services)
        : base(javaReference, transfer) => _services = services;

    public override void OnReceive(Context? _, Intent? intent)
    {
        if (intent?.Extras is not null)
        {
            var title   = intent.GetStringExtra(TitleKey.Default);
            var message = intent.GetStringExtra(MessageKey.Default);
            if (title is not null && message is not null)
            {
                _services.GetService<INotifications>()?.SendNotification(new(new(title, message)));
            }
        }
    }
}