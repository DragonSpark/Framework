using System;
using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace DragonSpark.Application.Mobile.Maui.Model.Navigation;

public class AsynchronousCommand<T> : IAsyncRelayCommand<T>
{
    readonly IAsyncRelayCommand<T> _previous;
    readonly TypeConverter?        _converter;

    protected AsynchronousCommand(Func<T?, Task> execute, TypeConverter? converter = null)
        : this(new AsyncRelayCommand<T>(execute), converter) {}

    protected AsynchronousCommand(Func<T?, Task> execute, Predicate<T?> canExecute, TypeConverter? converter = null)
        : this(new AsyncRelayCommand<T>(execute, canExecute), converter) {}

    public AsynchronousCommand(IAsyncRelayCommand<T> previous, TypeConverter? converter = null)
    {
        _previous  = previous;
        _converter = converter;
    }

    public Task ExecuteAsync(T? parameter) => _previous.ExecuteAsync(parameter);

    public bool CanExecute(object? parameter)
    {
        var input  = Input(parameter);
        var result = _previous.CanExecute(input);
        return result;
    }

    public void Execute(object? parameter)
    {
        var input = Input(parameter);
        _previous.Execute(input);
    }

    public event EventHandler? CanExecuteChanged
    {
        add => _previous.CanExecuteChanged += value;
        remove => _previous.CanExecuteChanged -= value;
    }

    public void NotifyCanExecuteChanged()
    {
        _previous.NotifyCanExecuteChanged();
    }

    public event PropertyChangedEventHandler? PropertyChanged
    {
        add => _previous.PropertyChanged += value;
        remove => _previous.PropertyChanged -= value;
    }

    public Task ExecuteAsync(object? parameter)
    {
        var input = Input(parameter);
        return _previous.ExecuteAsync(input);
    }

    object? Input(object? parameter)
    {
        if (parameter is not null)
        {
            return _converter?.CanConvertFrom(parameter.GetType()) ?? false
                       ? _converter.ConvertFrom(parameter)
                       : parameter;
        }

        return default(T);
    }

    public void Cancel() => _previous.Cancel();

    public Task? ExecutionTask => _previous.ExecutionTask;

    public bool CanBeCanceled => _previous.CanBeCanceled;

    public bool IsCancellationRequested => _previous.IsCancellationRequested;

    public bool IsRunning => _previous.IsRunning;

    public bool CanExecute(T? parameter) => _previous.CanExecute(parameter);

    public void Execute(T? parameter) => _previous.Execute(parameter);
}