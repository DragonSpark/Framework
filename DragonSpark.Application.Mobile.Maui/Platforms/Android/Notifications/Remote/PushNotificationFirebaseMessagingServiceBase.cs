using System.Threading;
using CommunityToolkit.Mvvm.Messaging;
using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;
using DragonSpark.Model.Operations.Stop;
using Firebase.Messaging;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Notifications.Remote;

public class PushNotificationFirebaseMessagingServiceBase : FirebaseMessagingService
{
    readonly IStopAware<string> _token;
    readonly IMessenger         _messenger;

    public PushNotificationFirebaseMessagingServiceBase() : this(SaveDeviceToken.Default, WeakReferenceMessenger.Default) {}

    public PushNotificationFirebaseMessagingServiceBase(IStopAware<string> token, IMessenger messenger)
    {
        _token     = token;
        _messenger = messenger;
    }

    public override void OnNewToken(string token)
    {
        _token.Get(new(token, CancellationToken.None)).AsTask().GetAwaiter().GetResult();
        _messenger.Send(new NewTokenReceivedMessage(token));
    }

    public override void OnMessageReceived(RemoteMessage message)
    {
        base.OnMessageReceived(message);

        if (message.Data.TryGetValue(ActionKey.Default, out var action))
        {
            _messenger.Send(new ActionReceivedMessage(action));
        }
    }
}