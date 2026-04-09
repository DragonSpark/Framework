using Android.Gms.Tasks;
using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;
using DragonSpark.Application.Mobile.Maui.Presentation;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Stop;
using Java.Lang;
using Microsoft.Extensions.Logging;
using CancellationToken = System.Threading.CancellationToken;
using Task = System.Threading.Tasks.Task;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Notifications.Remote;

sealed class TokenReceived : Object, IOnSuccessListener
{
    public static TokenReceived Default { get; } = new();

    TokenReceived() : this(SaveDeviceToken.Default) {}

    readonly IStopAware<string> _token;
    
    public TokenReceived(IStopAware<string> token) => _token = token;

    public void OnSuccess(Object? result)
    {
        var token = result?.ToString();
        _ = token is not null ? ProcessNewToken(token) : Task.CompletedTask;
    }
    
    async Task ProcessNewToken(string token)
    {
        try
        {
            await _token.Off(new(token, CancellationToken.None));
        }
        catch (Exception ex)
        {
            var logger = CurrentService<ILogger<PushNotificationFirebaseMessagingServiceBase>>.Default.Get();
            logger.LogError(ex, "Failed to process new FCM token");
        }
    }

}