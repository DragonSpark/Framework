using System.Windows.Input;
using DragonSpark.Application.Mobile.Maui.Model.Commands;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Text;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote.Messages;

public class ParsedActionRegistration<T> : Text.Text, IActionRegistration
{
    readonly IStopAware<T> _command;
    readonly IParser<T>     _parameter;

    protected ParsedActionRegistration(string name, IStopAware<T> command, Func<string, T> parser)
        : this(name, command, new Parser<T>(parser)) {}

    protected ParsedActionRegistration(string name, IStopAware<T> command, IParser<T> parameter) : base(name)
    {
        _command   = command;
        _parameter = parameter;
    }

    public ICommand Get(string? parameter)
    {
        var input = _parameter.Get(parameter.Verify());
        var body  = _command.Then().Bind(input).Get();
        return new StopAwareAsynchronousCommand<object>(x => body.Allocate(x));
    }
}