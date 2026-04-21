using System;
using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote.Messages;
using DragonSpark.Application.Mobile.Maui.Presentation;
using DragonSpark.Application.Mobile.Runtime.Initialization;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Stop;
using Firebase.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Notifications.Remote;

public class PushNotificationFirebaseMessagingServiceBase : FirebaseMessagingService
{
    readonly ICommand<IStopAware>       _register;
    readonly IStopAware<string>         _token;
    readonly Func<IProcessNotification> _process;

    public PushNotificationFirebaseMessagingServiceBase()
        : this(RegisterInitialization.Default, NewToken.Default,
               CurrentServices.Default.GetRequiredService<IProcessNotification>) {}

    public PushNotificationFirebaseMessagingServiceBase(ICommand<IStopAware> register, IStopAware<string> token,
                                                        Func<IProcessNotification> process)
    {
        _register = register;
        _token    = token;
        _process  = process;
    }

    public override void OnNewToken(string token)
    {
        _register.Execute(_token.Then().Bind(token).Out());
    }

    public override void OnMessageReceived(RemoteMessage message)
    {
        base.OnMessageReceived(message);

        var notification = message.GetNotification().Verify();
        var process      = _process();
        process.Execute(new(notification.Title ?? "Money Clouds Notification", notification.Body ?? string.Empty,
                            message.Data.TryGetValue(ActionKey.Default, out var action) ? action : string.Empty));
    }
}