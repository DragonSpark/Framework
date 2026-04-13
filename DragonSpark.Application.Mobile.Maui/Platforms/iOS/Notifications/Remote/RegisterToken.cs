using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;
using DragonSpark.Application.Mobile.Maui.Messaging;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Notifications.Remote;

public sealed class RegisterToken : IOperation<string>
{
    public static RegisterToken Default { get; } = new();

    RegisterToken() : this(SaveDeviceToken.Default, Send<NewTokenReceivedMessage>.Default) {}

    readonly IStopAware<string>                _token;
    readonly ICommand<NewTokenReceivedMessage> _new;

    public RegisterToken(IStopAware<string> token, ICommand<NewTokenReceivedMessage> @new)
    {
        _token = token;
        _new   = @new;
    }

    public async ValueTask Get(string parameter)
    {
        await _token.Off(new(parameter, CancellationToken.None));
        _new.Execute(new(parameter));
    }
}