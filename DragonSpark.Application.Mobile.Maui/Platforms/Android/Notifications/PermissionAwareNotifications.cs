using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android;
using DragonSpark.Application.Mobile.Maui.Device.Notifications;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.ApplicationModel;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Notifications;

sealed class PermissionAwareNotifications : INotifications
{
    readonly INotifications _previous;

    [ActivatorUtilitiesConstructor]
    public PermissionAwareNotifications(INotifications previous) => _previous = previous;

    public event EventHandler? NotificationReceived
    {
        add => _previous.NotificationReceived += value;
        remove => _previous.NotificationReceived -= value;
    }

    public async Task SendNotification(Token<NotificationInput> notification)
    {
        var granted = await Permissions.RequestAsync<NotificationPermission>().Off();
        switch (granted)
        {
            case PermissionStatus.Granted:
            case PermissionStatus.Restricted:
            case PermissionStatus.Limited:
                await _previous.SendNotification(notification).Off();
                break;
        }
    }

    public void ReceiveNotification(string title, string message)
    {
        _previous.ReceiveNotification(title, message);
    }
}

// TODO
public sealed class NotificationPermission : Permissions.BasePlatformPermission
{
    public override (string androidPermission, bool isRuntime)[] RequiredPermissions
    {
        get
        {
            var result = new List<(string androidPermission, bool isRuntime)>();
            if (!OperatingSystem.IsAndroidVersionAtLeast(33))
            {
                return result.ToArray();
            }

            result.Add((Manifest.Permission.PostNotifications, true));
            return result.ToArray();
        }
    }
}