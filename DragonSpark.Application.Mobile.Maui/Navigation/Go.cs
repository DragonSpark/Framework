using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
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