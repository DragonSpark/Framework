using System;
using DragonSpark.Application.Mobile.Uno.Device.Notifications;
using DragonSpark.Application.Mobile.Uno.Presentation;
using Microsoft.Extensions.DependencyInjection;
using UserNotifications;

namespace DragonSpark.Application.Mobile.Uno.Platforms.iOS.Notifications;

public sealed class NotificationReceiver : UNUserNotificationCenterDelegate
{
    readonly IServiceProvider                  _services;
    readonly UNNotificationPresentationOptions _options;

    public NotificationReceiver()
        : this(CurrentServices.Default, OperatingSystem.IsIOSVersionAtLeast(14)
                                            ? UNNotificationPresentationOptions.Banner
                                            : UNNotificationPresentationOptions.Alert) {}

    public NotificationReceiver(IServiceProvider services, UNNotificationPresentationOptions options)
    {
        _services = services;
        _options  = options;
    }

    // Called if app is in the foreground.
    public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification,
                                                 Action<UNNotificationPresentationOptions> completionHandler)
    {
        ProcessNotification(notification);

        completionHandler(_options);
    }

    // Called if app is in the background, or killed state.
    public override void DidReceiveNotificationResponse(UNUserNotificationCenter center,
                                                        UNNotificationResponse response, Action completionHandler)
    {
        if (response.IsDefaultAction)
        {
            ProcessNotification(response.Notification);
        }

        completionHandler();
    }

    void ProcessNotification(UNNotification notification)
    {
        var service = _services.GetService<INotifications>();
        if (service is not null)
        {
            var content = notification.Request.Content;
            service.ReceiveNotification(content.Title, content.Body);
        }
    }
}

// TODO