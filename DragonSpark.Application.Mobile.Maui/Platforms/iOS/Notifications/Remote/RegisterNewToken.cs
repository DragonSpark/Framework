using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;
using DragonSpark.Application.Mobile.Maui.Messaging;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Notifications.Remote;

sealed class RegisterNewToken : IStopAware<string>
{
    public static RegisterNewToken Default { get; } = new();

    RegisterNewToken() : this(SaveDeviceToken.Default, Send<NewTokenReceivedMessage>.Default) {}

    readonly IStopAware<string>                _token;
    readonly ICommand<NewTokenReceivedMessage> _new;

    public RegisterNewToken(IStopAware<string> token, ICommand<NewTokenReceivedMessage> @new)
    {
        _token = token;
        _new   = @new;
    }

    public async ValueTask Get(Stop<string> parameter)
    {
        await _token.Off(parameter);
        _new.Execute(new(parameter));
    }
}