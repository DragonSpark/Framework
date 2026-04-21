using CommunityToolkit.Mvvm.Input;
using DragonSpark.Application.Mobile.Maui.Model.Commands;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations.Stop;
using Command = Microsoft.Maui.Controls.Command;
using ICommand = System.Windows.Input.ICommand;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote.Messages;

public class ActionRegistration : Text.Text, IActionRegistration
{
    readonly ICommand _command;

    protected ActionRegistration(string name, IStopAware command)
        : this(name, new StopAwareAsynchronousCommand<None>(x => command.Allocate(x))) {}

    protected ActionRegistration(string name, ICommand command) : base(name) => _command = command;

    public ICommand Get(string? parameter) => _command;
}

public class ActionRegistration<T> : ActionRegistration
{
    protected ActionRegistration(string name, IRelayCommand<T> command, T parameter)
        : base(name, new Command(Start.A.Command<T>(command.Execute).Bind(parameter).Get().Execute)) {}
}