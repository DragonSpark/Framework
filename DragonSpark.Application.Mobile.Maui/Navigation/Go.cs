using System;
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

public class Go<T> : IOperation<T>
{
    readonly Shell                         _shell;
    readonly Func<T, ShellNavigationState> _state;

    protected Go(Func<T, ShellNavigationState> state) : this(Shell.Current, state) {}

    protected Go(Shell shell, Func<T, ShellNavigationState> state)
    {
        _shell = shell;
        _state = state;
    }

    public ValueTask Get(T parameter) => _shell.GoToAsync(_state(parameter)).ToOperation();
}