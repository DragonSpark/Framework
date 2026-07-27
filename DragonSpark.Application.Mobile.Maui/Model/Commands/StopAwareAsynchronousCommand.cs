using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Mobile.Maui.Model.Commands;

public class StopAwareAsynchronousCommand<T> : AsynchronousCommand<T>
{
    public StopAwareAsynchronousCommand(Func<Stop<T?>, Task> execute, TypeConverter? converter = null)
        : base(new AsyncRelayCommand<T>((x, stop) => execute(x.Stop(stop))), converter) {}

    protected StopAwareAsynchronousCommand(Func<Stop<T?>, Task> execute, Predicate<T?> canExecute,
                                           TypeConverter? converter = null)
        : base(new AsyncRelayCommand<T>((x, stop) => execute(x.Stop(stop)), canExecute), converter) {}
}