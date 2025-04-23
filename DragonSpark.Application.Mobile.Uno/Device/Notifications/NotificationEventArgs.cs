using System;

namespace DragonSpark.Application.Mobile.Uno.Device.Notifications;

public sealed class NotificationEventArgs : EventArgs
{
    public NotificationEventArgs(string title, string message)
    {
        Title   = title;
        Message = message;
    }

    public string Title { get; }

    public string Message { get; }
}