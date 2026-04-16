using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;
using DragonSpark.Application.Mobile.Maui.Messaging;
using DragonSpark.Application.Mobile.Runtime.Initialization;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Stop;
using Firebase.Messaging;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Notifications.Remote;

public class PushNotificationFirebaseMessagingServiceBase : FirebaseMessagingService
{
    readonly ICommand<IStopAware>           _register;
    readonly IStopAware<string>             _token;
    readonly ICommand<AlertReceivedMessage> _send;

    public PushNotificationFirebaseMessagingServiceBase()
        : this(RegisterInitialization.Default, NewToken.Default, Send<AlertReceivedMessage>.Default) {}

    public PushNotificationFirebaseMessagingServiceBase(ICommand<IStopAware> register, IStopAware<string> token,
                                                        ICommand<AlertReceivedMessage> send)
    {
        _register = register;
        _token    = token;
        _send     = send;
    }

    public override void OnNewToken(string token)
    {
        _register.Execute(_token.Then().Bind(token).Out());
    }

    public override void OnMessageReceived(RemoteMessage message)
    {
        base.OnMessageReceived(message);

        var notification = message.GetNotification().Verify();
        _send.Execute(new(notification.Title ?? "Money Clouds Notification", notification.Body ?? string.Empty,
                          message.Data.TryGetValue(ActionKey.Default, out var action) ? action : string.Empty));
    }
}