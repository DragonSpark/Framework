using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;
using DragonSpark.Application.Mobile.Maui.Messaging;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Stop;
using Firebase.Messaging;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Notifications.Remote;

public class PushNotificationFirebaseMessagingServiceBase : FirebaseMessagingService
{
    readonly IStopAware<string>                _token;
    readonly ICommand<NewTokenReceivedMessage> _new;
    readonly ICommand<ActionReceivedMessage>   _send;

    public PushNotificationFirebaseMessagingServiceBase()
        : this(SaveDeviceToken.Default, Send<NewTokenReceivedMessage>.Default, Send<ActionReceivedMessage>.Default) {}

    public PushNotificationFirebaseMessagingServiceBase(IStopAware<string> token,
                                                        ICommand<NewTokenReceivedMessage> @new,
                                                        ICommand<ActionReceivedMessage> send)
    {
        _token = token;
        _new   = @new;
        _send  = send;
    }

    /*public override void OnNewToken(string token)
    {
        _ = ProcessNewToken(token);
    }
    
    async Task ProcessNewToken(string token)
    {
        try
        {
            await _token.Off(new(token, CancellationToken.None));
            _new.Execute(new(token));
        }
        catch (Exception ex)
        {
            var logger = CurrentService<ILogger<PushNotificationFirebaseMessagingServiceBase>>.Default.Get();
            logger.LogError(ex, "Failed to process new FCM token");
        }
    }*/

    public override void OnMessageReceived(RemoteMessage message)
    {
        base.OnMessageReceived(message);

        if (message.Data.TryGetValue(ActionKey.Default, out var action))
        {
            _send.Execute(new(action));
        }
    }
}