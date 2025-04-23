using System;
using System.Threading.Tasks;
using Android;
using DragonSpark.Application.Mobile.Uno.Device.Notifications;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;
using Microsoft.Extensions.DependencyInjection;
using Windows.Extensions;

namespace DragonSpark.Application.Mobile.Uno.Platforms.Android.Notifications;

sealed class PermissionAwareNotifications : INotifications
{
    readonly INotifications _previous;
    readonly string                      _permission;

    [ActivatorUtilitiesConstructor]
    public PermissionAwareNotifications(INotifications previous)
        : this(previous, Manifest.Permission.PostNotifications) {}

    public PermissionAwareNotifications(INotifications previous, string permission)
    {
        _previous   = previous;
        _permission = permission;
    }

    public event EventHandler? NotificationReceived
    {
        add => _previous.NotificationReceived += value;
        remove => _previous.NotificationReceived -= value;
    }

    public async Task SendNotification(Token<NotificationInput> notification)
    {
        var (_, item) = notification;
        var granted = await PermissionsHelper.TryGetPermission(item, _permission).Off();
        if (granted)
        {
            await _previous.SendNotification(notification).Off();
        }
    }

    public void ReceiveNotification(string title, string message)
    {
        _previous.ReceiveNotification(title, message);
    }
}