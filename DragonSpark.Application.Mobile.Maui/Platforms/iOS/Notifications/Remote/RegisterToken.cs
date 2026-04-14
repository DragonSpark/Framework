using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;
using DragonSpark.Application.Security;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using DragonSpark.Text;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Notifications.Remote;

public sealed class RegisterToken : IOperation<Array<byte>>
{
    public static RegisterToken Default { get; } = new();

    RegisterToken() : this(DeviceToken.Default, HexContent.Default, RegisterNewToken.Default) {}

    readonly DragonSpark.Model.Operations.Results.Stop.IStopAware<string?> _current;
    readonly IFormatter<Array<byte>>                                       _formatter;
    readonly IOperation<string>                                            _register;

    public RegisterToken(DragonSpark.Model.Operations.Results.Stop.IStopAware<string?> current,
                         IFormatter<Array<byte>> formatter, IOperation<string> register)
    {
        _current   = current;
        _formatter = formatter;
        _register  = register;
    }

    public async ValueTask Get(Array<byte> parameter)
    {
        var current = await _current.Off(CancellationToken.None);
        var input   = _formatter.Get(parameter);
        if (current != input)
        {
            await _register.Off(input);
        }
    }
}