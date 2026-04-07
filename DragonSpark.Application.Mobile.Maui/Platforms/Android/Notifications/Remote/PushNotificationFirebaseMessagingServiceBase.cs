using System;
using System.Threading;
using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;
using DragonSpark.Application.Mobile.Maui.Messaging;
using DragonSpark.Application.Mobile.Maui.Presentation;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Stop;
using Firebase;
using Firebase.Messaging;
using Microsoft.Extensions.Logging;

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

    public override async void OnNewToken(string token) // Change to async void
    {
        var logger = CurrentService<ILogger<PushNotificationFirebaseMessagingServiceBase>>.Default.Get();
        logger.LogInformation("Hello World! OnNewToken");
        logger.LogInformation("Hello World! OnNewToken Sender: {Sender}", FirebaseApp.Instance.Options.GcmSenderId);

        try
        {
            // Await the asynchronous operation. Use ConfigureAwait(false) if the continuation doesn't need to return to the original context.
            await _token.Get(new(token, CancellationToken.None)).Off(); 
            _new.Execute(new(token)); // If _new.Execute is also async, you should await it too.
        }
        catch (Exception ex)
        {
            // Log any exceptions that occur during token processing
            logger.LogError(ex, "Error processing new FCM token in OnNewToken");
        }
    }

    public override void OnMessageReceived(RemoteMessage message)
    {
        CurrentService<ILogger<PushNotificationFirebaseMessagingServiceBase>>.Default.Get().LogInformation("OnMessageReceived! {Sender}", FirebaseApp.Instance.Options.GcmSenderId);
        
        base.OnMessageReceived(message);

        if (message.Data.TryGetValue(ActionKey.Default, out var action))
        {
            _send.Execute(new(action));
        }
    }
}