using System;
using DragonSpark.Model.Commands;
using Microsoft.Maui.ApplicationModel;

namespace DragonSpark.Application.Mobile.Maui.Runtime;

public class MainThreadAwareCommand<T> : ICommand<T>
{
    readonly Action<T> _previous;

    public MainThreadAwareCommand(ICommand<T> previous) : this(previous.Execute) {}

    protected MainThreadAwareCommand(Action<T> previous) => _previous = previous;

    public void Execute(T parameter)
    {
        MainThread.BeginInvokeOnMainThread(() => _previous(parameter));
    }
}