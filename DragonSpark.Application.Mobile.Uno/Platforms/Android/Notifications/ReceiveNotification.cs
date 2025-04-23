using System;
using Android.Content;
using DragonSpark.Application.Mobile.Uno.Device.Notifications;
using DragonSpark.Application.Mobile.Uno.Presentation;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Uno.Platforms.Android.Notifications;

public sealed class ReceiveNotification : ICommand<Intent>
{
    public static ReceiveNotification Default { get; } = new();

    ReceiveNotification() : this(CurrentServices.Default, TitleKey.Default, MessageKey.Default) {}

    readonly IServiceProvider _services;
    readonly string           _title, _message;

    public ReceiveNotification(IServiceProvider services, string title, string message)
    {
        _services = services;
        _title    = title;
        _message  = message;
    }

    public void Execute(Intent parameter)
    {
        var service = _services.GetService<INotifications>();
        if (service is not null)
        {
            var title   = parameter.GetStringExtra(_title);
            var message = parameter.GetStringExtra(_message);
            if (title is not null && message is not null)
            {
                service.ReceiveNotification(title, message);   
            }
        }
    }
}