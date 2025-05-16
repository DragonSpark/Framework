using System;
using System.Collections.Generic;
using Android;
using Microsoft.Maui.ApplicationModel;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Notifications;

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