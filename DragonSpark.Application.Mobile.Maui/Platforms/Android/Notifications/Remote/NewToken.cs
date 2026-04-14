using System;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;
using DragonSpark.Application.Mobile.Maui.Messaging;
using DragonSpark.Application.Mobile.Maui.Presentation;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Notifications.Remote;

sealed class NewToken : IOperation<string>
{
    public static NewToken Default { get; } = new();

    NewToken() : this(SaveDeviceToken.Default, Send<NewTokenReceivedMessage>.Default,
                      CurrentService<ILogger<NewToken>>.Default.Get) {}

    readonly IStopAware<string>                _token;
    readonly ICommand<NewTokenReceivedMessage> _new;
    readonly Func<ILogger<NewToken>>           _logger;

    public NewToken(IStopAware<string> token, ICommand<NewTokenReceivedMessage> @new, Func<ILogger<NewToken>> logger)
    {
        _token  = token;
        _new    = @new;
        _logger = logger;
    }

    public async ValueTask Get(string parameter)
    {
        try
        {
            await _token.Off(parameter.Stop());
            _new.Execute(new(parameter));
        }
        catch (Exception ex)
        {
            _logger().LogError(ex, "Failed to process new FCM token");
        }
    }
}