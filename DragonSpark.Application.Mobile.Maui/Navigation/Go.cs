using System;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Maui.Model.Navigation;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Navigation;

public class Go : IOperation
{
    readonly Shell                _shell;
    readonly ShellNavigationState _state;

    protected Go(ShellNavigationState state) : this(Shell.Current, state) {}

    protected Go(Shell shell, ShellNavigationState state)
    {
        _shell = shell;
        _state = state;
    }

    public ValueTask Get() => _shell.GoToAsync(_state).ToOperation();
}

// TODO

public sealed class Launch : AsynchronousCommand<Uri>
{
    public static Launch Default { get; } = new();

    Launch() : base(Launcher.OpenAsync!) {}
}