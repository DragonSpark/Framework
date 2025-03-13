using System;

namespace DragonSpark.Application.Mobile.Device.Notifications;

public readonly record struct NotificationInput(string Title, string Message, DateTime? NotifyTime = null);